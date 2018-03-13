using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Entity;
using XStore.Entity.Model;

namespace XStore.WebSite.WebSite.Information
{
    public partial class Achievement : CenterPage
    {
        public decimal yestodyTotal = 0;
        public decimal serverTotal = 0;
        public decimal lastMonthTotal = 0;
        public decimal thisMonthTotal = 0;
        public decimal thisYearTotal = 0;
        public decimal allTotal = 0;

        public decimal yestodyDaySales;
        public decimal serverDaySales;
        public decimal lastMonthDaySales;
        public decimal thisMonthDaySales;
        public decimal thisYearDaySales;
        public decimal allDaySales;
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-销售业绩";
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        protected void PageInit()
        {

            //昨日
            Yestoday.DataSource = Query(o => o.date.Date == DateTime.Now.AddDays(-1).Date && o.paid == true && o.price1 >= 1, out yestodyDaySales, out yestodyTotal);
            Yestoday.DataBind();
            //七天前
            ServerDays.DataSource = Query(o => o.date.Date >= DateTime.Now.AddDays(-7).Date && o.paid == true && o.price1 >= 1, out serverDaySales, out serverTotal);
            ServerDays.DataBind();
            //上月
            LastMonth.DataSource = Query(o => o.date.Year == DateTime.Now.AddMonths(-1).Year && o.date.Month == DateTime.Now.AddMonths(-1).Month&& o.paid == true && o.price1 >= 1, out lastMonthDaySales, out lastMonthTotal);
            LastMonth.DataBind();
            //本月
            ThisMonth.DataSource = Query(o => o.date.Year == DateTime.Now.Year && o.date.Month == DateTime.Now.Month && o.date.Day >= 1 && o.paid == true && o.price1 >= 1, out thisMonthDaySales, out thisMonthTotal);
            ThisMonth.DataBind();
            //今年
            ThisYear.DataSource = Query(o => o.date.Year == DateTime.Now.Year && o.paid == true && o.price1 >= 1, out thisYearDaySales, out thisYearTotal);
            ThisYear.DataBind();
            //全部
            AllGoods.DataSource = Query(o =>  o.paid == true && o.price1 >= 1, out allDaySales, out allTotal);
            AllGoods.DataBind();
        }


        protected List<SaleProduct> Query(Expression<Func<OrderInfo, bool>> _where,out decimal daySale, out decimal totalAmount)
        {
            var cabinetList = context.Query<Cabinet>().Where(o => o.hotel == hotelInfo.id).ToList();
            var query = context.Query<OrderInfo>().Where(_where).LeftJoin<Product>((a, b) => a.product == b.id)
                .Select((a, b) => new SaleProduct
                {
                    name = b.name,
                    code = b.code,
                    price1 = a.price1,
                    id = a.product,
                    saleTime = a.date
                });

            var groupQuery= query.GroupBy(o=>o.id).Select(o => new SaleProduct
                {
                    minTime = AggregateFunctions.Min(o.saleTime),
                    maxTime = AggregateFunctions.Max(o.saleTime),
                    name = AggregateFunctions.Max(o.name),
                    code = AggregateFunctions.Max(o.code),
                    salesCount = AggregateFunctions.Count(),
                    totalAmount = AggregateFunctions.Sum(o.price1)
                });
            var list = groupQuery.ToList();
            if (list.Count != 0)
            {
                 totalAmount = list.Sum(o => o.totalAmount).ObjToInt(0).CentToRMB(0);
                var totalDay = list.Max(o => o.maxTime).Subtract(list.Min(o => o.minTime)).Days;

                if (totalDay != 0 && cabinetList.Count != 0)
                {
                    daySale = Math.Round(totalAmount/ totalDay / cabinetList.Count);
                }
                else
                {
                    daySale = 0;
                }
            }
            else
            {
                daySale = 0;
                totalAmount = 0;
            }
            
            return list;
        }

    }

}