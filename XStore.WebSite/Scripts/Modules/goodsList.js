$(function(){
	var goodsListObj={
		init:function(){
			this.render();
			this.bindEvent();
			this.getData();
		},
		render:function(){
			$('#foot ul li').eq(2).addClass('clickOn').siblings().removeClass('clickOn');
		},
		bindEvent:function(){
			$('.topNav .goodsTitle').on('click',function(){
				$(this).css('border-color','#FF6600').siblings().css('border-color','#FFFFFF');
				$('.goods').show();
				$('.distributer').hide();
			});
			$('.topNav .destributerTitle').on('click',function(){
				$(this).css('border-color','#FF6600').siblings().css('border-color','#FFFFFF');
				$('.goods').hide();
				$('.distributer').show();
			})
		},
		getData:function(){
			var hr=window.location.href;
			if(hr.split('?')[1]==1){
				$('.topNav .r').css('border-color','#FF6600');
				$('.goods').hide();
				$('.distributer').show();
			}else{
				$('.topNav .l').css('border-color','#FF6600');
				$('.goods').show();
				$('.distributer').hide();
			}
		}
	};
	goodsListObj.init();
})