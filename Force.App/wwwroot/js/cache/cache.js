//缓存操作
var cache = {
    //存储
    set(key, value) {
        localStorage.setItem(key, JSON.stringify(value));
    },
    //取出
    get(key) {
        var userStr = localStorage.getItem(key);
        try {
            userStr =
                pageInitModule.decodeUnicode(JSON.parse(localStorage.getItem(key)));
        } catch (e) {

        }
        return JSON.parse(userStr);
    },
    getString(key) {

        var userStr = localStorage.getItem(key);
        try {
            userStr =
                pageInitModule.decodeUnicode(JSON.parse(localStorage.getItem(key)));
        } catch (e) {

        }
        return userStr;
    },
    //删除指定key
    remove_key(key) {
        localStorage.removeItem(key);
    },
    //清空
    clear_all() {
        localStorage.clear();
    }
}
 