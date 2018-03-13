$(function(){
	var pickUpObj={
		init:function(){
			this.render();
			this.bindEvent();
			this.getData();
		},
		render:function(){
			$('#foot ul li').eq(1).addClass('clickOn').siblings().removeClass('clickOn');
		},
		bindEvent:function(){
			
		},
		getData:function(){
			
		}
	};
	pickUpObj.init();
})