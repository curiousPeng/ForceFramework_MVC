var serverSestting = {
    resourceUrl: "",//资源地址
    resourceReleaseUrl: "http://116.62.23.242:8203",//资源发布地址
    resourceDebugUrl: "http://192.168.50.193:8203",//资源开发地址
    resourceLocalUrl: "http://192.168.50.193:8203",//资源开发地址
    serverUrl: "http://192.168.1.1"//服务器地址
}
var paramObj = pageInitModule.getJsParam(); 
if (paramObj['environment'] == undefined || paramObj['environment'] == "debug") {
    serverSestting.resourceUrl = serverSestting.resourceDebugUrl;
} else if (paramObj['environment'] == "release") {
    serverSestting.resourceUrl = serverSestting.resourceReleaseUrl;
} else if (paramObj['environment'] == "local") {
    serverSestting.resourceUrl = serverSestting.resourceLocalUrl;
}