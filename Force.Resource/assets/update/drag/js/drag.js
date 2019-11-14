/* 
 * drag 1.0
 * create by CuriousPeng@outlook.com
 * date 2015-08-18
 * 拖动滑块
 */
(function($){
    $.fn.drag = function(options){
        var x, drag = this, isMove = false, defaults = {
        };
        var options = $.extend(defaults, options);
        //添加背景，文字，滑块
        var html = '<div class="drag_bg"></div>'+
                    '<div class="drag_text" onselectstart="return false;" unselectable="on">拖动滑块登录</div>'+
                    '<div class="handler handler_bg"></div>';
        this.append(html);
        
        var handler = drag.find('.handler');
        var drag_bg = drag.find('.drag_bg');
        var text = drag.find('.drag_text');
        var maxWidth = drag.width() - handler.width();  //能滑动的最大间距
        //鼠标按下时候的x轴的位置
        handler.mousedown(function(e){
            isMove = true;
            x = e.pageX - parseInt(handler.css('left'), 10);
        });
       
        //鼠标指针在上下文移动时，移动距离大于0小于最大间距，滑块x轴位置等于鼠标移动距离
        $(document).mousemove(function(e){
            var _x = e.pageX - x;
            if(isMove){
                if(_x > 0 && _x <= maxWidth){
                    handler.css({'left': _x});
                    drag_bg.css({'width': _x});
                }else if(_x > maxWidth && vailWidth()){  //鼠标指针移动距离达到最大时清空事件
                 if ($('.login-form').validate().form()) {
                        dragOk();
                        //$('.login-form').submit(); //form validation success, call ajax form submit
                     App.blockUI();
                        $.ajax({
                                url:$(".login-form").attr("action"),
                                type:"post",
                                dataType:"json",
                                data:{account:$("input[name='username']").val(),pwd:$("input[name='password']").val()},
                                success:function(result){
                                    App.unblockUI();
                                    if(result.status==1){
                                         text.text(result.msg);
                                         if(result.pwdStrength<2){
                                            window.location.href="/home/firstlogin";
                                         }else{
                                            window.location.href="/productpackage/index";
                                         }
                                    }else{
                                         clearDrag();
                                         addAlert("#bootstrap_alerts", "danger",result.msg);
                                    }
                                },
                                errror:function(){
                                    clearDrag();
                                     addAlert("#bootstrap_alerts", "danger","登录请求失败！请检查网络或联系管理员！");
                                }
                        });
                    }else{
                        clearDrag();
                    }
                }
            }
        }).mouseup(function(e){
            isMove = false;
            var _x = e.pageX - x;
            if(_x < maxWidth){ //鼠标松开时，如果没有达到最大距离位置，滑块就返回初始位置
                handler.css({'left': 0});
                drag_bg.css({'width': 0});
            }
        });
     function vailWidth(){
        var drag_width = drag.width();
        var handler_left = parseInt(handler.css('left'), 10);
        var handler_width = handler.width();
        var move_width = drag_width-handler_left-5;
        if(move_width<=handler_width){
            return true;
        }
        isMove = false;
        handler.css({'left': 0});
        drag_bg.css({'width': 0});
        return false;
     }
     function clearDrag(){
        isMove = false;
        handler.css({'left': 0});
        drag_bg.css({'width': 0});
        handler.removeClass('handler_ok_bg').addClass('handler_bg');
        text.text('拖动滑块登录');
        drag.css({ 'color': '' });
        //$('#drag').drag();
     }
     var addAlert = function (container, type, msg) {
        App.alert({
            container: container, // alerts parent container(by default placed after the page breadcrumbs)
            type: type,  // alert's type
            message: msg,  // alert's message
            close: "true", // make alert closable
            closeInSeconds: 5 // auto close after defined seconds
        });
    }

        //清空事件
        function dragOk(){
            handler.removeClass('handler_bg').addClass('handler_ok_bg');
            text.text('验证通过正在登录...');
            drag.css({ 'color': '#fff' });
            isMove = false;
            //handler.unbind('mousedown');
            //$(document).unbind('mousemove');
            //$(document).unbind('mouseup');
        }
    };
})(jQuery);


