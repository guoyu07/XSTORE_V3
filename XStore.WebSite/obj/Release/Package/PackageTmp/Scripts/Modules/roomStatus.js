$(function(){
	var roomStatusObj={
		init:function(){
			this.render();
			this.bindEvent();
			this.getData();
		},
		render:function(){
			$('#foot ul li').eq(0).addClass('clickOn').siblings().removeClass('clickOn');
		},
		bindEvent:function(){
			
		},
		getData:function(){
			
		}
	};
	roomStatusObj.init();
})