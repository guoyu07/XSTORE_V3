$(function(){
	var homeObj={
		init:function(){
			this.render();
			this.bindEvent();
			this.getData();
		},
		render:function(){
			$('#foot ul li').eq(0).addClass('clickOn').siblings().removeClass('clickOn');
		},
		bindEvent:function(){
			$('.cancellation').on('click',function(){
				$('.tips').show();
			});
			$('.cancel').on('click',function(){
				$('.tips').hide();
			})
		},
		getData:function(){
			
		}
	};
	homeObj.init();
})
