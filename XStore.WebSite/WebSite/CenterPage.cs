using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XStore.Entity;

namespace XStore.WebSite.WebSite
{
    public class CenterPage : BasePage
    {
        #region 用户信息
        private User _userinfo;
        public User userInfo
        {
            get
            {
                if (_userinfo == null)
                {
                    _userinfo = context.Query<User>().FirstOrDefault(o => o.weichat.Equals(OpenId));
                    if (_userinfo != null)
                    {
                        Session[Constant.CurrentUser] = _userinfo;
                    }
                }
                return _userinfo;
            }
        }
        #endregion

        #region 用户权限
        private UserRole _userRole;

        public UserRole userRole
        {
            get {
                if (_userRole == null)
                {
                    _userRole = context.Query<UserRole>().FirstOrDefault(o => o.username.Equals(userInfo.username));
                }
                return _userRole;
            }
        }
        #endregion

        #region 微信用户信息
        private UserWeiChat _wxuserInfo;
        public UserWeiChat wxUserInfo
        {
            get
            {
                if (_wxuserInfo == null)
                {
                    _wxuserInfo = context.Query<UserWeiChat>().FirstOrDefault(o => o.openid.Equals(userInfo.weichat));
                    if (_wxuserInfo != null)
                    {
                        Session[Constant.WeiUser] = _wxuserInfo;
                    }
                }
                return _wxuserInfo;
            }
           
        }
        #endregion

        #region 酒店信息
        private Hotel _hotelInfo;
        public Hotel hotelInfo
        {
            get
            {
                if (_hotelInfo == null)
                {
                    if (Session[Constant.HotelId].ObjToInt(0)==0)
                    {
                        var hotelId = Request.QueryString[Constant.HotelId].ObjToInt(0);
                        if (hotelId != 0)
                        {
                            _hotelInfo = context.Query<Hotel>().FirstOrDefault(o => o.id == hotelId);
                            Session[Constant.HotelId] = _hotelInfo.id;
                        }
                        else
                        {
                            _hotelInfo = context.Query<Hotel>().LeftJoin<UserHotel>((a, b) => a.id == b.hotels_id).Where((a, b) => b.user_username.Equals(userInfo.username)).Select((a, b) => new Hotel
                            {
                                id = a.id,
                                hotel_name = a.hotel_name,
                                simple_name = a.simple_name,
                                address = a.address
                            }).FirstOrDefault();
                            Session[Constant.HotelId] = _hotelInfo.id;
                        }
                    }
                    else
                    {
                        var hotelId = Session[Constant.HotelId].ObjToInt(0);
                        _hotelInfo = context.Query<Hotel>().FirstOrDefault(o => o.id == hotelId);
                    }
                    
                }
                return _hotelInfo;
            }
        }
        #endregion
    }
}