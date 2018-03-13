using Chloe;
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
    public partial class RoomFillUp : CenterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-常规补货";
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        protected void PageInit()
        {
            try
            {
                if (userInfo == null)
                {
                    MessageBox.Show(this, "system_alert", "用户未绑定");
                    return;
                }
                else
                {
                    if (hotelInfo == null)
                    {
                        MessageBox.Show(this, "system_alert", "酒店信息异常");
                        return;
                    }
                    else
                    {
                        var layout = context.Query<CabinetLayout>().FirstOrDefault(o => o.hotel_id == hotelInfo.id);
                        if (layout == null)
                        {
                            MessageBox.Show(this, "system_alert", "模板商品未配置");
                            return;
                        }
                        var productCount = layout.products.Split(new char[] { ','},StringSplitOptions.RemoveEmptyEntries).Count();

                        var roomList = context.Query<Cabinet>().Where(o => o.hotel == hotelInfo.id)
                            .LeftJoin<Cell>((a, b) => a.mac.Equals(b.mac))
                            .Where((a, b) => b.part == 0 && b.product_id == null && b.pos <= productCount)
                            .Select((a, b) => new
                            {
                                a.mac,
                                a.online,
                                a.room
                            }).GroupBy(o => o.mac)
                        .Select(o => new { mac = AggregateFunctions.Max(o.mac), online = AggregateFunctions.Max(o.online), room = AggregateFunctions.Max(o.room) }).ToList();
                        rooms_rp.DataSource = roomList;
                        rooms_rp.DataBind();
                        if (roomList.Count() > 0)
                        {
                            roomSelectDiv.Visible = true;
                            empty_div.Visible = false;
                        }
                        else
                        {
                            roomSelectDiv.Visible = false;
                            empty_div.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "system_alert", "数据异常");
            }
        }

        protected void markSure_Click(object sender, EventArgs e)
        {
            try
            {
                var macsStr = room_id.Value;
                var cabinets = context.Query<Cabinet>().Where(o => macsStr.Contains(o.mac)).ToList();
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
                    MessageBox.Show(this, "system_alert", "无补货房间");
                    return;
                }
                //补货的商品id列表
                var fixProductList = new List<string>();
                var cabinetLayOut = context.Query<CabinetLayout>().FirstOrDefault(o => o.hotel_id == hotelInfo.id);
                if (cabinetLayOut == null || string.IsNullOrEmpty(cabinetLayOut.products))
                {
                    MessageBox.Show(this, "system_alert", "商品模板未设置");
                    return;
                }
                var layoutList = cabinetLayOut.products.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(o => o.ObjToInt(0)).Where(o => o != 0).ToList();

                for (int i = 0; i < cabinets.Count; i++)
                {
                    var cabinet = cabinets[i];
                    var storeList = context.Query<Cell>().Where(o => o.mac.Equals(cabinet.mac) && o.part == 0 && o.pos <= layoutList.Count).ToList();

                    if (storeList.Count<layoutList.Count )
                    {
                        MessageBox.Show(this, "system_alert", "房间【" + cabinet.room + "】商品设置不全");
                        return;
                    }
                    for (int j = 0; j < layoutList.Count; j++)
                    {
                        if (storeList[j].product_id == null)
                        {
                            fixProductList.Add(layoutList[j].ToString());
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
    }
}