$(function(){
	var myselfObj={
		init:function(){
			this.render();
		},
		render:function(){
			$('#foot ul li').eq(1).addClass('clickOn').siblings().removeClass('clickOn');
            $("a[name='con']").eq(1).addClass("on");
		}
	}
	myselfObj.init();
})
