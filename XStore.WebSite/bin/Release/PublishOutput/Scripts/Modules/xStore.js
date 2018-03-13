$(function(){
	var myselfObj={
		init:function(){
			this.render();
		},
		bindEvent:function(){
			
		},
		render:function(){
			console.log(123);
			$('#foot ul li').eq(3).addClass('clickOn').siblings().removeClass('clickOn');
		}
	}
	myselfObj.init();
})