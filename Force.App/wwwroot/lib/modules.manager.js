// 模块管理
var modules = (function Manager() {
    var modules = {};

    /**
     * 定义模块
     * @param {String} name 模块的名称
     * @param {String[]} deps 该模块需要依赖的模块名称
     * @param {Function} impl 模块实现 
     */
    function define(name, deps, impl) {
        for (var i = 0; i < deps.length; i++) {
            //获取依赖的模块
            deps[i] = modules[deps[i]];
        }
        /**
         * 设置模块的依赖项，通过参数进行扩展         
        */
        modules[name] = impl.apply(impl, deps);
    }

    /**
     * 通过名称获取模块
     * @param {String} name 模块名称
     * @returns {Function} 模块 
     */
    function get(name) {
        return modules[name];
    }

    return {
        define: define,
        get: get
    };
})();