using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using XStore.Common;
using XStore.Entity;
using XStore.Entity.Model;

namespace XStore.WebSite.WebSite.Goods
{
    public partial class GoodsList : MacPage
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间";
            if (!IsPostBack)
            {
                if (cabinet ==null)
                {
                    MessageBox.Show(this, "system_alert", "箱子未绑定房间");
                    return;
                }
                if (cabinet.online.HasValue)//离线
                {
                    if (!cabinet.online.Value)
                    {
                        Response.Redirect(string.Format(Constant.LoginDic + "NoPower.aspx?boxmac = {0}", cabinet.mac), false);
                        return;
                    }
                    else
                    {
                        PageInit();
                    }
                   
                }
               
            }
        }
        protected void PageInit()
        {
            try
            {
                #region 绑定房间商品
                BindGoods(this,goods_list,cabinet);
                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }

        }
       
        protected string link_detail(bool sell_out,int position,int product_id)
        {
           
            if (sell_out)
            {
                return "#";
            }
            else
            {
                return string.Format("Detail.aspx?product_id={0}&boxmac={1}&position={2}", product_id, cabinet.mac,position);
            }

        }
    }
}