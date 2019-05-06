//折叠ibox
$('.collapse-link').click(function () {
    var ibox = $(this).closest('div.ibox');
    var button = $(this).find('i');
    var content = ibox.find('div.ibox-content');
    content.slideToggle(200);
    button.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
    ibox.toggleClass('').toggleClass('border-bottom');
    setTimeout(function () {
        ibox.resize();
        ibox.find('[id^=map-]').resize();
    }, 50);
});

//关闭ibox
$('.close-link').click(function () {
    var content = $(this).closest('div.ibox');
    content.remove();
});



// 赋值表单
function setFromElements(fm, data) {
    /// <signature>
    ///     <summary> 赋值表单 </summary>
    ///     <param name="fm" type="string,object" > 表单id,表单对象 </param>
    ///     <param name="data" type="object" > json对象 </param>
    /// </signature>
    var fmcds;
    if (typeof (fm) == 'string') fmcds = $(fId + ' :input');
    else if (typeof (fm) == 'object') fmcds = fm.find(':input');
    $.each(fmcds, function (k, v) {
        if (v.type == 'button') return;
        if (v.type == 'select-one') {
            $(this).val(data[v.name] == undefined ? '' : data[v.name]).change();
            return;
        }
        if (v.type == 'radio') {
            $(this).prop('checked', $(this).val() == data[v.name]);
        }
        if (v.type == 'text' || v.type == 'hidden') {
            this.value = data[v.name] == undefined ? '' : data[v.name];
            return;
        }
        if (v.type == 'textarea') {
            $(this).text(data[v.name] == undefined ? '' : data[v.name]);
            $(this).val($(this).text());
            return;
        }
    });
}

// 清空表单元素的值
function clearFormElements(fId) {
    $.each($(fId + ' :input'), function (k, v) {
        console.log(v.type)
        if ($(this).attr('data-clear') == "false") return; // 忽略清空
        if (v.type == 'button') return;
        if (v.type == 'select-one') {
            $(v).val('');
            return;
        }
        if (v.type == 'radio') {
            $(this).prop('checked', false);
            return;
        }
        if (v.type == 'text' || v.type == 'hidden' || v.type == 'password') {
            this.value = '';
            return;
        }
        if (v.type == 'textarea') {
            $(this).val('');
            return;
        }
    });
}

//序列化表单为json对象
function getJsonObjectByForm(fId, removenull) {
    /// <signature>
    ///     <summary> 序列化表单为json对象 </summary>
    ///     <param name="fid" type="String" > 表单id </param>
    ///     <param name="removenull" type="boolean" > 是否去掉空值 </param>
    /// </signature>
    var json = {}
   //获取到表下标签 json格式的数据
   , array = $(fId).serializeArray()
   , i, length, key, value;
    for (i = 0, length = array.length; i < length; i++) {
        key = array[i].name;
        value = array[i].value;
        if (value == "" && removenull == undefined) continue; // 去掉空值
        if (json[key] != undefined) { // 相同name
            json[key] += "," + value;
        }
        else json[key] = value;
    }
    return json;
}
// 序列化表单为json字符串
function getJsonByForm(fId) {
    return JSON.stringify(getJsonObjectByForm(fId));
}

// 清空键值对json,保留指定键值
function clearJsonObject(d, igk) {
    var data = {};
    $.each(d, function (k, v) {
        if (igk.indexOf == undefined && igk[k] != undefined) { // 键值对
            data[k] = v;
            return;
        }
        else if (igk.indexOf != undefined && igk.indexOf(k) != -1) { //键名数组 不要清空的键
            data[k] = v;
            return;
        }
        data[k] = '';
    });
    return data;
}

//定时器模拟文本框 change 事件
function txtChange(id, func) {
    /// <signature>
    ///     <summary> 定时器模拟文本框 change 事件 </summary>
    ///     <param name="id" type="int" > 表单id </param>
    ///     <param name="func" type="event" > 值改变回调事件 </param>
    /// </signature>
    $(id).data('oldval',$(id).val()); 
    $(id).focus(function () {  
        var setId = setInterval(function () { 
            if ($(id).val() == $(id).data('oldval')) return; 
            $(id).data('oldval', $(id).val()); 
            func(id); //change 事件 
        }, 300); 
        $(this).data('civid', setId); 

    }).blur(function () { 
        clearInterval($(this).data('civid')); 
    });  
}
//绑定示例：
//txtChange('#sehMonth', function (id) {
//    console.log(id + '--' + $(id).val());
//});
