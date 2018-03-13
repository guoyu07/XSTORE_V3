$(function(){
	var historyNewsObj={
		init:function(){
			this.render();
			this.bindEvent();
			this.getData();
		},
		render:function(){
			$('#foot ul li').eq(3).addClass('clickOn').siblings().removeClass('clickOn');
		},
		bindEvent:function(){
			$('ul').on('click','li',function(){
				$(this).toggleClass('showContent').find('dl').toggle();
			});
		},
		getData:function(){
			
		}
	};
	historyNewsObj.init();
})