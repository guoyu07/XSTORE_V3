using Chloe.MySql;
using log4net;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Common.WeiXinPay;
using XStore.Entity;
using XStore.Entity.Model;
using XStore.WebSite.DBFactory;

namespace XStore.WebSite
{
    public class BasePage:System.Web.UI.Page
    {
        
        public static string connString = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        protected bool debug = bool.Parse(ConfigurationManager.AppSettings["DEBUG"].ObjToStr());
        protected string homeUrl = ConfigurationManager.AppSettings["HomeUrl"].ObjToStr();
        public MySqlContext context ;
        public ILog Log;
        public BasePage() {
            context = new MySqlContext(new MySqlConnectionFactory(connString));
            Log = log4net.LogManager.GetLogger("Weixin.Logging");//获取一个日志记录器
            Log.Info(DateTime.Now.ToString() + ": login success");//写入一条新log
        }
        protected override void OnInit(EventArgs e)
        {
            var InitOpenid = OpenId;
            base.OnInit(e);
        }
        private string _openid;
        protected string OpenId
        {
            get
            {
                if (debug)
                {
                    _openid = "ooZJm0e-HAspMBhNrw0bUGXD-k6M";//袁
                    //_openid = "ooZJm0d_Cimev2TQHdCJGq4LOlHU";//储
                    //_openid = "ooZJm0Z0wg3kmeht0e4u40pgKuq4";//小号
                }
                LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "openid：");
                if (_openid == null || string.IsNullOrEmpty(_openid))
                {
                    if (!string.IsNullOrEmpty(Request.QueryString[Constant.OpenId].ObjToStr()))
                    {
                        _openid = Request.QueryString[Constant.OpenId].ObjToStr();
                        LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "_openid：" + _openid);
                        Session[Constant.OpenId] = _openid;
                    }
                    else
                    {
                        if (Session[Constant.OpenId] == null || string.IsNullOrEmpty(Session[Constant.OpenId].ObjToStr()))
                        {
                            _openid = RedrectWeiXin();
                        }
                        else
                        {
                            _openid = Session[Constant.OpenId].ObjToStr();
                        }
                    }
                   
                }
                else
                {
                    Session[Constant.OpenId] = _openid;
                }
                return _openid;
            }
        }
        private string _accessToken;
        protected string accessToken
        {
            get
            {
                
                if (_accessToken == null || string.IsNullOrEmpty(_accessToken))
                {
                    if (Session[Constant.AccessToken] == null || string.IsNullOrEmpty(Session[Constant.AccessToken].ObjToStr()))
                    {
                        var requestUrl = Constant.YunApiV2 + "Shop/ashx/GetAccessToken.ashx";
                        var response = JsonConvert.DeserializeObject<AjaxResponse>(Utils.HttpGet(requestUrl));
                        if (response.success)
                        {
                            Session[Constant.AccessToken] = response.message.ObjToStr();
                            _accessToken = response.message.ObjToStr();
                        }
                        else
                        {
                            MessageBox.Show(this, "system_alert", "获取AccessToken失败");
                        }
                    }
                    else
                    {
                        _accessToken = Session[Constant.AccessToken].ObjToStr();
                    }
                }
                else
                {
                    Session[Constant.AccessToken] = _accessToken;
                }
                return _accessToken;
            }

        }

        protected string RedrectWeiXin()
        {
            try
            {
                LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "重新获取openid：");
                string url = HttpContext.Current.Request.Url.AbsolutePath;
                string query = HttpContext.Current.Request.Url.Query;
                string RedirectUri = homeUrl + url + query;
                WeiXinOath wxOath = new WeiXinOath();
                WxUserInfo wxUserInfo = new WxUserInfo();
                if (Session == null || string.IsNullOrEmpty(Session[Constant.OpenId].ObjToStr()))
                {
                    LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "OpenId不存在：");
                    var code = Request.QueryString[Constant.WxCode];
                    LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "code："+ code);
                    #region 根据code获取openid
                    if (code != null && !string.IsNullOrEmpty(code))
                    {
                        OauthToken oathToken = new OauthToken();
                        oathToken = wxOath.GetOauthToken(code);//获取用户openid
                        Session[Constant.OpenId] = oathToken.openid;
                        LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "openid：" + oathToken.openid);
                        #region 存入用户信息
                        wxUserInfo = wxOath.GetWebUserInfo(accessToken, oathToken.openid);
                        LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"存入用户信息："+ JsonConvert.SerializeObject(wxUserInfo));
                        var wxUserDB = context.Query<UserWeiChat>().FirstOrDefault(o => o.openid.Equals(oathToken.openid));
                        if (wxUserDB == null)
                        {
                            context.Insert(new UserWeiChat
                            {
                                createtime = DateTime.Now,
                                headpic = wxUserInfo.headimgurl,
                                nickname = wxUserInfo.nickname,
                                openid = wxUserInfo.openid,
                                unionid = string.Empty
                            });
                        }
                        else
                        {
                            wxUserDB.headpic = wxUserInfo.headimgurl;
                            wxUserDB.nickname = wxUserInfo.nickname;
                            context.Update(wxUserDB);
                        }
                        #endregion
                        return oathToken.openid;
                    }
                    else
                    {
                        wxOath.GetCode(RedirectUri);
                        return string.Empty;
                    }
                    #endregion
                }
                {
                    return Session[Constant.OpenId].ObjToStr();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "数据异常：" + ex.InnerException.Message);
                MessageBox.Show(this, "system_alert", "数据异常:" + ex.Message + ":" + ex.StackTrace);
                return string.Empty;
            }

        }
       

        public string GetProductImg(int productId,string image) {
            return "/Source/product/" + productId + "/" + image;

        }
        public string GetProductHtml( string html)
        {
            return "/Source/html/"+ html + ".html";

        }

        #region 获取商品列表
        public List<int> BindGoods(System.Web.UI.Page page,Repeater repeater, Cabinet cabinet)
        {
            var proidlist = new List<int>();
            var layout = context.Query<CabinetLayout>().FirstOrDefault(o => o.hotel_id == cabinet.hotel);
            if (layout == null)
            {
                MessageBox.Show(page, "system_alert", "酒店未设置默认商品");
                return proidlist;
            }
            var layoutProList = layout.products.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(o => o.ObjToInt(0)).Where(o => o != 0).ToList();

            var proidList = new List<int>();

            proidList = context.Query<Cell>().Where(o => o.part == 0 && o.mac.Equals(cabinet.mac)).Select(o => o.product_id.HasValue ? o.product_id.Value : 0).ToList();
            if (proidList.Count == 0)
            {
                proidList = new List<int>();
                //如果mac对应的商品没有配置
                for (int i = 0; i < layoutProList.Count; i++)
                {
                    proidList.Add(0);
                }
            }

            if (proidList.Count() < layoutProList.Count())
            {
                MessageBox.Show(this, "system_alert", "房间设置商品不全");
                return proidlist;
            }
            List<ProductQuery> list = new List<ProductQuery>();
            List<Product> productList = context.Query<Product>().Where(o => o.state == 1).ToList();

            for (int i = 0; i < layoutProList.Count(); i++)
            {
                var proid = proidList[i].ObjToInt(0);
                var sell_out = false;
                //如果实际商品是0，则用默认商品补全
                if (proid == 0)
                {
                    proid = layoutProList[i].ObjToInt(0);
                    sell_out = true;
                }
                var pro = productList.FirstOrDefault(o => o.id == proid);
                if (pro == null)
                {
                    continue;
                }
                var proQuery = TinyMapper.Map<ProductQuery>(pro);
                proQuery.sell_out = sell_out;
                list.Add(proQuery);
            }
            repeater.DataSource = list;
            repeater.DataBind();
            return proidList;
        }

        #endregion

    }
}