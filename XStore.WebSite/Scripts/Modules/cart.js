$(function(){
	var cartObj={
		init:function(){
			this.render();
			this.bindEvent();
			this.getData();
		},
		render:function(){
			$('#foot ul li').eq(1).addClass('clickOn').siblings().removeClass('clickOn');
		},
		bindEvent: function () {
		    var me = this;
			$('.editor').on('click',function(){
				$('.del').show();
				$(this).hide();
				$('.finish').show();
			});
			$('.finish').on('click',function(){
				$(this).hide();
				$('.editor').show();
				$('.del').hide();
			});
			$('ul').on('click','li .del',function(){
			    var that=this;
			    layer.open({
			        content:'是否删除该商品？',
			        btn:['确认','取消'],
			        yes: function (index) {
			            var num = $(that).parents('li').find('#cart_id').text();
			            console.log(num);
			            $.ajax({
                            url: '../ashx/CartDel.ashx',
			                type: 'get',
			                data: {
			                    CartId: num
			                },
                            dataType:'json',
			                success: function (result) {
			                    // window.location.reload();
			                    layer.close(index);
			                    if(result.state==1){
			                        $('.totalMoney span').text(result.info);
			                        $(that).parent().remove();
			                        me.isEmpty();
			                    }else{
			                        layer.open({
			                            content: '删除失败',
                                        time: 2,
                                        skin: 'msg'
			                        });
                                        
			                    }
			                }
			            })
					}
				})
			});
		},
		isEmpty: function () {
		    console.log($('.goodsBox').children());
		    if ($('.goodsBox').children().length == 0) {
		        $('.hasGoods').hide();
		        $('.empty').show();
		        $('.editor').show();
		        $('.finish').hide();
		    } else {
		        $('.hasGoods').show();
		        $('.empty').hide();
		    }
		},
		getData:function(){

		}
	};
	cartObj.init();
})