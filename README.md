# MoocDownload
### 下载视js脚本
```javascript
var selector = 'a.J-media-item';
var videoes = [];
var xmlStr = '<?xml version="1.0" encoding="utf-8" ?><videoes>';
var dict = {};
var total = $(selector).length;
var textStr = '';
$(selector).each(function(i, e) {
    var href = this.href;
    var vid = href.substring(href.lastIndexOf('/') + 1, href.length); // this.href.replace('http://www.imooc.com/video/', '');
    var name = this.innerText;
    var pattern = /\(\d{2}:\d{2}\)/;
    if (!pattern.test(name)) {
        total--;
        if (i == $(selector).length - 1 && !total) {
            console.log('没有视频可以下载！');
        }
        return;
    };
    name = name.replace(/\(\d{2}:\d{2}\)/, '').replace(/\s/g, '');
    //name += '.mp4';
    dict[vid] = name;
    $.getJSON("/course/ajaxmediainfo/?mid=" + vid + "&mode=flash", function(data) {
        var url = data.data.result.mpath[2];
        videoes.push({
            url: url,
            name: name
        });
        xmlStr += '<video><url>' + url + '</url><name>' + name + '</name></video>';
        textStr += 'filename=' + name + '&fileurl=' + url + '\n';
        if (videoes.length == total) {
            console.log('共' + total + '个视频。');
            console.log('已完成' + videoes.length + '个视频。');
            //console.log(JSON.stringify(videoes));
            xmlStr += '</videoes>';
            //console.log(xmlStr);
            console.log(textStr);
            console.log($('.hd .l').text());
        };
    });
});

//下载代码文件
var arr=[],fileStr='';
$('.downlist li').each(function(){
    var model={
        name:$(this.children[0]).text(),
        url:this.children[1].href
    };
    arr.push(model);
    fileStr+="filename="+model.name+"&fileurl="+model.url+"\n";
});
```
