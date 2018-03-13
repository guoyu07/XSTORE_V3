using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Entity;
using XStore.Entity.Model;

namespace XStore.WebSite.WebSite.Operation
{
    public partial class PickUp : CenterPage
    {
        #region 房间
        private List<Cabinet> _cabinets;
        protected List<Cabinet> cabinets
        {
            get
            {
                if (_cabinets == null)
                {
                    var macsStr = Request.QueryString[Constant.IMEIS].ObjToStr();
                    Session[Constant.Cabinets] = macsStr;
                    if (!string.IsNullOrEmpty(macsStr))
                    {
                        _cabinets = context.Query<Cabinet>().Where(o => macsStr.Contains(o.mac)).ToList();

                    }
                    else
                    {
                        _cabinets = new List<Cabinet>();
                    }
                }
                return _cabinets;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-常规补货";
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        private void PageInit()
        {
            try
            {
                if (userInfo == null)
                {
                    MessageBox.Show(this, "system_alert", "用户不存在");
                    return; 
                }
                var userRole = context.Query<UserRole>().FirstOrDefault(o => o.username.Equals(userInfo.username));
                if (userRole == null)
                {
                    MessageBox.Show(this, "system_alert", "用户未设定权限");
                    return;
                }
                if (cabinets.Count == 0)
                {
                    MessageBox.Show(this,"system_alert","无补货房间");
                    return;
                }
                //补货的商品id列表
                var fixProductList = new List<string>();
                var cabinetLayOut = context.Query<CabinetLayout>().FirstOrDefault(o => o.hotel_id == hotelInfo.id);
                if (cabinetLayOut == null||string.IsNullOrEmpty(cabinetLayOut.products))
                {
                    MessageBox.Show(this,"system_alert","商品模板未设置");
                    return;
                }
                var layoutList = cabinetLayOut.products.Split(new char[] { ','},StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int i = 0; i < cabinets.Count; i++)
                {
                    var cabinet = cabinets[i];
                    var storeList = context.Query<Cell>().Where(o=>o.mac.Equals(cabinet.mac)&&o.part == 0).ToList();
   
                    if (layoutList.Count != storeList.Count)
                    {
                        MessageBox.Show(this, "system_alert", "房间【" + cabinet.room + "】商品设置不全");
                        return;
                    }
                    for (int j = 0; j < storeList.Count; j++)
                    {
                        if (storeList[j].product_id==null)
                        {
                            fixProductList.Add(layoutList[j]);
                        }
                    } 
                }
                var productGroup = fixProductList.GroupBy(o => o).ToList();
                var bindProductList = new List<Product>();
                foreach (var product in productGroup)
                {
                    var productId = product.Key.ObjToInt(0);
                    var productDB = context.Query<Product>().FirstOrDefault(o => o.id == productId);
                    if (productDB == null)
                    {
                        MessageBox.Show(this, "system_alert", "商品【" + product.Key.ObjToInt(0) + "】不存在");
                        return;
                    }
                    var fixedProduct = TinyMapper.Map<FixedProductQuery>(productDB);
                    fixedProduct.count = product.Count();
                    bindProductList.Add(fixedProduct);
                   
                }
                Rp_pickup.DataSource = bindProductList;
                Rp_pickup.DataBind();
                if (bindProductList.Count > 0)
                {
                    pickUp.Visible = true;
                    noTask.Visible = false;
                }
                else
                {
                    pickUp.Visible = false;
                    noTask.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "补货房间异常：" + ex.Message + ";内部异常：" + ex.InnerException.Message);
                MessageBox.Show(this, "system_alert", "数据异常");
                return;
            }

        }
        protected void markSure_OnServerClick(object sender, EventArgs e)
        {
            MessageBox.Show(this,"system_alert","取货完成");
            Response.Redirect(Constant.OperationDic + "FinishPickUp.aspx", false);
        }
    }
}