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
    public partial class Settlement : CenterPage
    {
        public string start_time = string.Empty;
        public string end_time = string.Empty;
        public int data_type = 0;
        public decimal totalAmount;
        public int totalSum;
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-业绩查询";
            start_time = Request.QueryString["start_time"].ObjToStr();
            end_time = Request.QueryString["end_time"].ObjToStr();
            data_type = Request.QueryString["data_type"].ObjToInt(0);

            if (!IsPostBack)
            {
                PageInit();
            }
        }
        protected void PageInit()
        {
            var start = string.IsNullOrEmpty(start_time.ObjToStr())||start_time.Equals("-起始时间-") ? DateTime.MinValue:DateTime.Parse(start_time);
            var end = string.IsNullOrEmpty(end_time.ObjToStr())||end_time.Equals("-截止时间-") ? DateTime.MaxValue : DateTime.Parse(end_time);
            notlement.DataSource = Query(o=>o.date>= start && o.date < end.AddDays(1) && o.paid== true && o.price1 >= 1, out totalSum, out totalAmount);
            notlement.DataBind();

            data_type_input.Value = data_type.ObjToStr();
            start_date_label.InnerText = string.IsNullOrEmpty(start_time) ? "-起始时间-" : start_time;
            end_date_label.InnerText = string.IsNullOrEmpty(end_time) ? "-截止时间-" : end_time;

        }

        protected List<SaleProduct> Query(Expression<Func<OrderInfo, bool>> _where, out int totalSum, out decimal totalAmount)
        {
            var query = context.Query<OrderInfo>().Where(_where).LeftJoin<Product>((a, b) => a.product == b.id)
                .Select((a, b) => new SaleProduct
                {
                    name = b.name,
                    code = b.code,
                    price1 = a.price1,
                    id = a.product,
                    saleTime = a.date
                });

            var groupQuery = query.GroupBy(o => o.id).Select(o => new SaleProduct
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

                totalSum = list.Sum(o => o.salesCount).ObjToInt(0);
            }
            else
            {
                totalSum = 0;
                totalAmount = 0;
            }

            return list;
        }
    }
}