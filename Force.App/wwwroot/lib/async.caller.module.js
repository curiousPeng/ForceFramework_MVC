function AsyncCaller() {
    var completeCallback = undefined;               // 执行完后回调方法
    var errorMsg = "同一时间只能有一个任务被运行";  // 错误信息
    var allTasksPool = [];                          // 并行任务池
    var queueTasksPool = [];                        // 队列任务池
    var execType = "queue";                         // 执行类型：默认队列

    /**
     * 通知队列异步方法执行完毕
     */
    function notifyAsyncComplate() {
        // 只有队列任务才执行下一个方法
        if (execType === "queue")
            aysncFunRunOver();
    }

    /**
     * 添加并行方法
     * @param {Function} func 要执行的方法
     * @param {Object} arguArr 方法的参数
     * @returns {AsyncCaller} 当前AsyncCaller实例
     */
    function pushAll(func, arguArr) {
        if (queueTasksPool.length === 0) {
            var funcObj = { 'func': func, 'argu': arguArr };
            allTasksPool.push(funcObj);
        } else {
            console.error(errorMsg);
        }
        return this;
    };

    /**
     * 添加异步方法到队列
     * @param {Function} asyncFunc 异步方法
     * @param {Object} arguArr 参数
     * @returns {AsyncCaller} 当前AsyncCaller实例
     */
    function pushQueue(asyncFunc, arguArr) {        
        if (allTasksPool.length === 0) {            
            var funcObj = { 'func': asyncFunc, 'argu': arguArr };
            queueTasksPool.push(funcObj);
        } else {
            console.error(errorMsg);
        }
        return this;
    };

    /**
     * 队列异步方法已执行完毕后的操作
     */
    function aysncFunRunOver() {
        if (queueTasksPool.length === 0) {
            if (completeCallback) {
                completeCallback();
            }
        } else {
            var funcObj = queueTasksPool.shift();
            if (funcObj !== undefined)
                funcObj.func(funcObj.argu);
        }
    };

    /**
     * 执行方法
     * @param {Function} callback 方法执行完后的回调方法
     */
    function exec(callback) {
        completeCallback = callback;
        if (allTasksPool.length > 0) {
            execType = "all";
            executeAll();
        } else if (queueTasksPool.length > 0) {
            execType = "queue";
            executeQueue();
        } else {
            completeCallback();
            completeCallback = undefined;
        }
    };

    /**
     * 执行队列方法
     */
    function executeQueue() {
        var funcObj = queueTasksPool.shift();
        if (funcObj !== undefined)
            funcObj.func(funcObj.argu);
    }

    /**
     * 执行所有方法
     */
    function executeAll() {
        for (var i = 0; i < allTasksPool.length; i++) {
            var funcObj = allTasksPool[i];
            if (funcObj !== undefined)
                funcObj.func(funcObj.argu);
        }
        if (completeCallback)
            completeCallback();
    }

    /**
     * 清空所有方法
     */
    function cleanAllFuc() {
        allTasksPool = [];
        queueTasksPool = [];
    }

    return {
        exec: exec,
        pushAll: pushAll,
        pushQueue: pushQueue,
        notifyQueueAsyncFuncComplate: notifyAsyncComplate,
        cleanAllFuc: cleanAllFuc
    };
}