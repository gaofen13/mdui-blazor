/*!
 * mdui 1.0.2 (https://mdui.org)
 * Copyright 2016-2023 zdhxiong
 * Licensed under MIT
 */
function isFunction(target) {
    return typeof target === 'function';
}
function isString(target) {
    return typeof target === 'string';
}
function isNumber(target) {
    return typeof target === 'number';
}
function isBoolean(target) {
    return typeof target === 'boolean';
}
function isUndefined(target) {
    return typeof target === 'undefined';
}
function isNull(target) {
    return target === null;
}
function isWindow(target) {
    return target instanceof Window;
}
function isDocument(target) {
    return target instanceof Document;
}
function isElement(target) {
    return target instanceof Element;
}
function isNode(target) {
    return target instanceof Node;
}
/**
 * 是否是 IE 浏览器
 */
function isIE() {
    // @ts-ignore
    return !!window.document.documentMode;
}
function isArrayLike(target) {
    if (isFunction(target) || isWindow(target)) {
        return false;
    }
    return isNumber(target.length);
}
function isObjectLike(target) {
    return typeof target === 'object' && target !== null;
}
function toElement(target) {
    return isDocument(target) ? target.documentElement : target;
}
/**
 * 把用 - 分隔的字符串转为驼峰（如 box-sizing 转换为 boxSizing）
 * @param string
 */
function toCamelCase(string) {
    return string
        .replace(/^-ms-/, 'ms-')
        .replace(/-([a-z])/g, (_, letter) => letter.toUpperCase());
}
/**
 * 把驼峰法转为用 - 分隔的字符串（如 boxSizing 转换为 box-sizing）
 * @param string
 */
function toKebabCase(string) {
    return string.replace(/[A-Z]/g, (replacer) => '-' + replacer.toLowerCase());
}
/**
 * 获取元素的样式值
 * @param element
 * @param name
 */
function getComputedStyleValue(element, name) {
    return window.getComputedStyle(element).getPropertyValue(toKebabCase(name));
}
/**
 * 检查元素的 box-sizing 是否是 border-box
 * @param element
 */
function isBorderBox(element) {
    return getComputedStyleValue(element, 'box-sizing') === 'border-box';
}
/**
 * 获取元素的 padding, border, margin 宽度（两侧宽度的和，单位为px）
 * @param element
 * @param direction
 * @param extra
 */
function getExtraWidth(element, direction, extra) {
    const position = direction === 'width' ? ['Left', 'Right'] : ['Top', 'Bottom'];
    return [0, 1].reduce((prev, _, index) => {
        let prop = extra + position[index];
        if (extra === 'border') {
            prop += 'Width';
        }
        return prev + parseFloat(getComputedStyleValue(element, prop) || '0');
    }, 0);
}
/**
 * 获取元素的样式值，对 width 和 height 进行过处理
 * @param element
 * @param name
 */
function getStyle(element, name) {
    // width、height 属性使用 getComputedStyle 得到的值不准确，需要使用 getBoundingClientRect 获取
    if (name === 'width' || name === 'height') {
        const valueNumber = element.getBoundingClientRect()[name];
        if (isBorderBox(element)) {
            return `${valueNumber}px`;
        }
        return `${valueNumber -
            getExtraWidth(element, name, 'border') -
            getExtraWidth(element, name, 'padding')}px`;
    }
    return getComputedStyleValue(element, name);
}
/**
 * 获取子节点组成的数组
 * @param target
 * @param parent
 */
function getChildNodesArray(target, parent) {
    const tempParent = document.createElement(parent);
    tempParent.innerHTML = target;
    return [].slice.call(tempParent.childNodes);
}
/**
 * 始终返回 false 的函数
 */
function returnFalse() {
    return false;
}
/**
 * 数值单位的 CSS 属性
 */
const cssNumber = [
    'animationIterationCount',
    'columnCount',
    'fillOpacity',
    'flexGrow',
    'flexShrink',
    'fontWeight',
    'gridArea',
    'gridColumn',
    'gridColumnEnd',
    'gridColumnStart',
    'gridRow',
    'gridRowEnd',
    'gridRowStart',
    'lineHeight',
    'opacity',
    'order',
    'orphans',
    'widows',
    'zIndex',
    'zoom',
];

function each(target, callback) {
    if (isArrayLike(target)) {
        for (let i = 0; i < target.length; i += 1) {
            if (callback.call(target[i], i, target[i]) === false) {
                return target;
            }
        }
    }
    else {
        const keys = Object.keys(target);
        for (let i = 0; i < keys.length; i += 1) {
            if (callback.call(target[keys[i]], keys[i], target[keys[i]]) === false) {
                return target;
            }
        }
    }
    return target;
}

/**
 * 为了使用模块扩充，这里不能使用默认导出
 */
class JQ {
    constructor(arr) {
        this.length = 0;
        if (!arr) {
            return this;
        }
        each(arr, (i, item) => {
            // @ts-ignore
            this[i] = item;
        });
        this.length = arr.length;
        return this;
    }
}

function get$() {
    const $ = function (selector) {
        if (!selector) {
            return new JQ();
        }
        // JQ
        if (selector instanceof JQ) {
            return selector;
        }
        // function
        if (isFunction(selector)) {
            if (/complete|loaded|interactive/.test(document.readyState) &&
                document.body) {
                selector.call(document, $);
            }
            else {
                document.addEventListener('DOMContentLoaded', () => selector.call(document, $), false);
            }
            return new JQ([document]);
        }
        // String
        if (isString(selector)) {
            const html = selector.trim();
            // 根据 HTML 字符串创建 JQ 对象
            if (html[0] === '<' && html[html.length - 1] === '>') {
                let toCreate = 'div';
                const tags = {
                    li: 'ul',
                    tr: 'tbody',
                    td: 'tr',
                    th: 'tr',
                    tbody: 'table',
                    option: 'select',
                };
                each(tags, (childTag, parentTag) => {
                    if (html.indexOf(`<${childTag}`) === 0) {
                        toCreate = parentTag;
                        return false;
                    }
                    return;
                });
                return new JQ(getChildNodesArray(html, toCreate));
            }
            // 根据 CSS 选择器创建 JQ 对象
            const isIdSelector = selector[0] === '#' && !selector.match(/[ .<>:~]/);
            if (!isIdSelector) {
                return new JQ(document.querySelectorAll(selector));
            }
            const element = document.getElementById(selector.slice(1));
            if (element) {
                return new JQ([element]);
            }
            return new JQ();
        }
        if (isArrayLike(selector) && !isNode(selector)) {
            return new JQ(selector);
        }
        return new JQ([selector]);
    };
    $.fn = JQ.prototype;
    return $;
}
const $ = get$();

const mdui = {
    $: $,
};

$.fn.each = function (callback) {
    return each(this, callback);
};

/**
 * 检查 container 元素内是否包含 contains 元素
 * @param container 父元素
 * @param contains 子元素
 * @example
```js
contains( document, document.body ); // true
contains( document.getElementById('test'), document ); // false
contains( $('.container').get(0), $('.contains').get(0) ); // false
```
 */
function contains(container, contains) {
    return container !== contains && toElement(container).contains(contains);
}

/**
 * 把第二个数组的元素追加到第一个数组中，并返回合并后的数组
 * @param first 第一个数组
 * @param second 该数组的元素将被追加到第一个数组中
 * @example
```js
merge( [ 0, 1, 2 ], [ 2, 3, 4 ] )
// [ 0, 1, 2, 2, 3, 4 ]
```
 */
function merge(first, second) {
    each(second, (_, value) => {
        first.push(value);
    });
    return first;
}

$.fn.get = function (index) {
    return index === undefined
        ? [].slice.call(this)
        : this[index >= 0 ? index : index + this.length];
};

$.fn.find = function (selector) {
    const foundElements = [];
    this.each((_, element) => {
        merge(foundElements, $(element.querySelectorAll(selector)).get());
    });
    return new JQ(foundElements);
};

// 存储事件
const handlers = {};
// 元素ID
let mduiElementId = 1;
/**
 * 为元素赋予一个唯一的ID
 */
function getElementId(element) {
    const key = '_mduiEventId';
    // @ts-ignore
    if (!element[key]) {
        // @ts-ignore
        element[key] = ++mduiElementId;
    }
    // @ts-ignore
    return element[key];
}
/**
 * 解析事件名中的命名空间
 */
function parse(type) {
    const parts = type.split('.');
    return {
        type: parts[0],
        ns: parts.slice(1).sort().join(' '),
    };
}
/**
 * 命名空间匹配规则
 */
function matcherFor(ns) {
    return new RegExp('(?:^| )' + ns.replace(' ', ' .* ?') + '(?: |$)');
}
/**
 * 获取匹配的事件
 * @param element
 * @param type
 * @param func
 * @param selector
 */
function getHandlers(element, type, func, selector) {
    const event = parse(type);
    return (handlers[getElementId(element)] || []).filter((handler) => handler &&
        (!event.type || handler.type === event.type) &&
        (!event.ns || matcherFor(event.ns).test(handler.ns)) &&
        (!func || getElementId(handler.func) === getElementId(func)) &&
        (!selector || handler.selector === selector));
}
/**
 * 添加事件监听
 * @param element
 * @param types
 * @param func
 * @param data
 * @param selector
 */
function add(element, types, func, data, selector) {
    const elementId = getElementId(element);
    if (!handlers[elementId]) {
        handlers[elementId] = [];
    }
    // 传入 data.useCapture 来设置 useCapture: true
    let useCapture = false;
    if (isObjectLike(data) && data.useCapture) {
        useCapture = true;
    }
    types.split(' ').forEach((type) => {
        if (!type) {
            return;
        }
        const event = parse(type);
        function callFn(e, elem) {
            // 因为鼠标事件模拟事件的 detail 属性是只读的，因此在 e._detail 中存储参数
            const result = func.apply(elem, 
            // @ts-ignore
            e._detail === undefined ? [e] : [e].concat(e._detail));
            if (result === false) {
                e.preventDefault();
                e.stopPropagation();
            }
        }
        function proxyFn(e) {
            // @ts-ignore
            if (e._ns && !matcherFor(e._ns).test(event.ns)) {
                return;
            }
            // @ts-ignore
            e._data = data;
            if (selector) {
                // 事件代理
                $(element)
                    .find(selector)
                    .get()
                    .reverse()
                    .forEach((elem) => {
                    if (elem === e.target ||
                        contains(elem, e.target)) {
                        callFn(e, elem);
                    }
                });
            }
            else {
                // 不使用事件代理
                callFn(e, element);
            }
        }
        const handler = {
            type: event.type,
            ns: event.ns,
            func,
            selector,
            id: handlers[elementId].length,
            proxy: proxyFn,
        };
        handlers[elementId].push(handler);
        element.addEventListener(handler.type, proxyFn, useCapture);
    });
}
/**
 * 移除事件监听
 * @param element
 * @param types
 * @param func
 * @param selector
 */
function remove(element, types, func, selector) {
    const handlersInElement = handlers[getElementId(element)] || [];
    const removeEvent = (handler) => {
        delete handlersInElement[handler.id];
        element.removeEventListener(handler.type, handler.proxy, false);
    };
    if (!types) {
        handlersInElement.forEach((handler) => removeEvent(handler));
    }
    else {
        types.split(' ').forEach((type) => {
            if (type) {
                getHandlers(element, type, func, selector).forEach((handler) => removeEvent(handler));
            }
        });
    }
}

$.fn.trigger = function (type, extraParameters) {
    const event = parse(type);
    let eventObject;
    const eventParams = {
        bubbles: true,
        cancelable: true,
    };
    const isMouseEvent = ['click', 'mousedown', 'mouseup', 'mousemove'].indexOf(event.type) > -1;
    if (isMouseEvent) {
        // Note: MouseEvent 无法传入 detail 参数
        eventObject = new MouseEvent(event.type, eventParams);
    }
    else {
        eventParams.detail = extraParameters;
        eventObject = new CustomEvent(event.type, eventParams);
    }
    // @ts-ignore
    eventObject._detail = extraParameters;
    // @ts-ignore
    eventObject._ns = event.ns;
    return this.each(function () {
        this.dispatchEvent(eventObject);
    });
};

function extend(target, object1, ...objectN) {
    objectN.unshift(object1);
    each(objectN, (_, object) => {
        each(object, (prop, value) => {
            if (!isUndefined(value)) {
                target[prop] = value;
            }
        });
    });
    return target;
}

/**
 * 将数组或对象序列化，序列化后的字符串可作为 URL 查询字符串使用
 *
 * 若传入数组，则格式必须和 serializeArray 方法的返回值一样
 * @param obj 对象或数组
 * @example
```js
param({ width: 1680, height: 1050 });
// width=1680&height=1050
```
 * @example
```js
param({ foo: { one: 1, two: 2 }})
// foo[one]=1&foo[two]=2
```
 * @example
```js
param({ids: [1, 2, 3]})
// ids[]=1&ids[]=2&ids[]=3
```
 * @example
```js
param([
  {"name":"name","value":"mdui"},
  {"name":"password","value":"123456"}
])
// name=mdui&password=123456
```
 */
function param(obj) {
    if (!isObjectLike(obj) && !Array.isArray(obj)) {
        return '';
    }
    const args = [];
    function destructure(key, value) {
        let keyTmp;
        if (isObjectLike(value)) {
            each(value, (i, v) => {
                if (Array.isArray(value) && !isObjectLike(v)) {
                    keyTmp = '';
                }
                else {
                    keyTmp = i;
                }
                destructure(`${key}[${keyTmp}]`, v);
            });
        }
        else {
            if (value == null || value === '') {
                keyTmp = '=';
            }
            else {
                keyTmp = `=${encodeURIComponent(value)}`;
            }
            args.push(encodeURIComponent(key) + keyTmp);
        }
    }
    if (Array.isArray(obj)) {
        each(obj, function () {
            destructure(this.name, this.value);
        });
    }
    else {
        each(obj, destructure);
    }
    return args.join('&');
}

// 全局配置参数
const globalOptions = {};
// 全局事件名
const ajaxEvents = {
    ajaxStart: 'start.mdui.ajax',
    ajaxSuccess: 'success.mdui.ajax',
    ajaxError: 'error.mdui.ajax',
    ajaxComplete: 'complete.mdui.ajax',
};

/**
 * 判断此请求方法是否通过查询字符串提交参数
 * @param method 请求方法，大写
 */
function isQueryStringData(method) {
    return ['GET', 'HEAD'].indexOf(method) >= 0;
}
/**
 * 添加参数到 URL 上，且 URL 中不存在 ? 时，自动把第一个 & 替换为 ?
 * @param url
 * @param query
 */
function appendQuery(url, query) {
    return `${url}&${query}`.replace(/[&?]{1,2}/, '?');
}
/**
 * 合并请求参数，参数优先级：options > globalOptions > defaults
 * @param options
 */
function mergeOptions(options) {
    // 默认参数
    const defaults = {
        url: '',
        method: 'GET',
        data: '',
        processData: true,
        async: true,
        cache: true,
        username: '',
        password: '',
        headers: {},
        xhrFields: {},
        statusCode: {},
        dataType: 'text',
        contentType: 'application/x-www-form-urlencoded',
        timeout: 0,
        global: true,
    };
    // globalOptions 中的回调函数不合并
    each(globalOptions, (key, value) => {
        const callbacks = [
            'beforeSend',
            'success',
            'error',
            'complete',
            'statusCode',
        ];
        // @ts-ignore
        if (callbacks.indexOf(key) < 0 && !isUndefined(value)) {
            defaults[key] = value;
        }
    });
    return extend({}, defaults, options);
}
/**
 * 发送 ajax 请求
 * @param options
 * @example
```js
ajax({
  method: "POST",
  url: "some.php",
  data: { name: "John", location: "Boston" }
}).then(function( msg ) {
  alert( "Data Saved: " + msg );
});
```
 */
function ajax(options) {
    // 是否已取消请求
    let isCanceled = false;
    // 事件参数
    const eventParams = {};
    // 参数合并
    const mergedOptions = mergeOptions(options);
    let url = mergedOptions.url || window.location.toString();
    const method = mergedOptions.method.toUpperCase();
    let data = mergedOptions.data;
    const processData = mergedOptions.processData;
    const async = mergedOptions.async;
    const cache = mergedOptions.cache;
    const username = mergedOptions.username;
    const password = mergedOptions.password;
    const headers = mergedOptions.headers;
    const xhrFields = mergedOptions.xhrFields;
    const statusCode = mergedOptions.statusCode;
    const dataType = mergedOptions.dataType;
    const contentType = mergedOptions.contentType;
    const timeout = mergedOptions.timeout;
    const global = mergedOptions.global;
    // 需要发送的数据
    // GET/HEAD 请求和 processData 为 true 时，转换为查询字符串格式，特殊格式不转换
    if (data &&
        (isQueryStringData(method) || processData) &&
        !isString(data) &&
        !(data instanceof ArrayBuffer) &&
        !(data instanceof Blob) &&
        !(data instanceof Document) &&
        !(data instanceof FormData)) {
        data = param(data);
    }
    // 对于 GET、HEAD 类型的请求，把 data 数据添加到 URL 中
    if (data && isQueryStringData(method)) {
        // 查询字符串拼接到 URL 中
        url = appendQuery(url, data);
        data = null;
    }
    /**
     * 触发事件和回调函数
     * @param event
     * @param params
     * @param callback
     * @param args
     */
    function trigger(event, params, callback, ...args) {
        // 触发全局事件
        if (global) {
            $(document).trigger(event, params);
        }
        // 触发 ajax 回调和事件
        let result1;
        let result2;
        if (callback) {
            // 全局回调
            if (callback in globalOptions) {
                // @ts-ignore
                result1 = globalOptions[callback](...args);
            }
            // 自定义回调
            if (mergedOptions[callback]) {
                // @ts-ignore
                result2 = mergedOptions[callback](...args);
            }
            // beforeSend 回调返回 false 时取消 ajax 请求
            if (callback === 'beforeSend' &&
                (result1 === false || result2 === false)) {
                isCanceled = true;
            }
        }
    }
    // XMLHttpRequest 请求
    function XHR() {
        let textStatus;
        return new Promise((resolve, reject) => {
            // GET/HEAD 请求的缓存处理
            if (isQueryStringData(method) && !cache) {
                url = appendQuery(url, `_=${Date.now()}`);
            }
            // 创建 XHR
            const xhr = new XMLHttpRequest();
            xhr.open(method, url, async, username, password);
            if (contentType ||
                (data && !isQueryStringData(method) && contentType !== false)) {
                xhr.setRequestHeader('Content-Type', contentType);
            }
            // 设置 Accept
            if (dataType === 'json') {
                xhr.setRequestHeader('Accept', 'application/json, text/javascript');
            }
            // 添加 headers
            if (headers) {
                each(headers, (key, value) => {
                    // undefined 值不发送，string 和 null 需要发送
                    if (!isUndefined(value)) {
                        xhr.setRequestHeader(key, value + ''); // 把 null 转换成字符串
                    }
                });
            }
            // 检查是否是跨域请求，跨域请求时不添加 X-Requested-With
            const crossDomain = /^([\w-]+:)?\/\/([^/]+)/.test(url) &&
                RegExp.$2 !== window.location.host;
            if (!crossDomain) {
                xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
            }
            if (xhrFields) {
                each(xhrFields, (key, value) => {
                    // @ts-ignore
                    xhr[key] = value;
                });
            }
            eventParams.xhr = xhr;
            eventParams.options = mergedOptions;
            let xhrTimeout;
            xhr.onload = function () {
                if (xhrTimeout) {
                    clearTimeout(xhrTimeout);
                }
                // AJAX 返回的 HTTP 响应码是否表示成功
                const isHttpStatusSuccess = (xhr.status >= 200 && xhr.status < 300) ||
                    xhr.status === 304 ||
                    xhr.status === 0;
                let responseData;
                if (isHttpStatusSuccess) {
                    if (xhr.status === 204 || method === 'HEAD') {
                        textStatus = 'nocontent';
                    }
                    else if (xhr.status === 304) {
                        textStatus = 'notmodified';
                    }
                    else {
                        textStatus = 'success';
                    }
                    if (dataType === 'json') {
                        try {
                            responseData =
                                method === 'HEAD' ? undefined : JSON.parse(xhr.responseText);
                            eventParams.data = responseData;
                        }
                        catch (err) {
                            textStatus = 'parsererror';
                            trigger(ajaxEvents.ajaxError, eventParams, 'error', xhr, textStatus);
                            reject(new Error(textStatus));
                        }
                        if (textStatus !== 'parsererror') {
                            trigger(ajaxEvents.ajaxSuccess, eventParams, 'success', responseData, textStatus, xhr);
                            resolve(responseData);
                        }
                    }
                    else {
                        responseData =
                            method === 'HEAD'
                                ? undefined
                                : xhr.responseType === 'text' || xhr.responseType === ''
                                    ? xhr.responseText
                                    : xhr.response;
                        eventParams.data = responseData;
                        trigger(ajaxEvents.ajaxSuccess, eventParams, 'success', responseData, textStatus, xhr);
                        resolve(responseData);
                    }
                }
                else {
                    textStatus = 'error';
                    trigger(ajaxEvents.ajaxError, eventParams, 'error', xhr, textStatus);
                    reject(new Error(textStatus));
                }
                // statusCode
                each([globalOptions.statusCode, statusCode], (_, func) => {
                    if (func && func[xhr.status]) {
                        if (isHttpStatusSuccess) {
                            func[xhr.status](responseData, textStatus, xhr);
                        }
                        else {
                            func[xhr.status](xhr, textStatus);
                        }
                    }
                });
                trigger(ajaxEvents.ajaxComplete, eventParams, 'complete', xhr, textStatus);
            };
            xhr.onerror = function () {
                if (xhrTimeout) {
                    clearTimeout(xhrTimeout);
                }
                trigger(ajaxEvents.ajaxError, eventParams, 'error', xhr, xhr.statusText);
                trigger(ajaxEvents.ajaxComplete, eventParams, 'complete', xhr, 'error');
                reject(new Error(xhr.statusText));
            };
            xhr.onabort = function () {
                let statusText = 'abort';
                if (xhrTimeout) {
                    statusText = 'timeout';
                    clearTimeout(xhrTimeout);
                }
                trigger(ajaxEvents.ajaxError, eventParams, 'error', xhr, statusText);
                trigger(ajaxEvents.ajaxComplete, eventParams, 'complete', xhr, statusText);
                reject(new Error(statusText));
            };
            // ajax start 回调
            trigger(ajaxEvents.ajaxStart, eventParams, 'beforeSend', xhr);
            if (isCanceled) {
                reject(new Error('cancel'));
                return;
            }
            // Timeout
            if (timeout > 0) {
                xhrTimeout = setTimeout(() => {
                    xhr.abort();
                }, timeout);
            }
            // 发送 XHR
            xhr.send(data);
        });
    }
    return XHR();
}

$.ajax = ajax;

/**
 * 为 Ajax 请求设置全局配置参数
 * @param options 键值对参数
 * @example
```js
ajaxSetup({
  dataType: 'json',
  method: 'POST',
});
```
 */
function ajaxSetup(options) {
    return extend(globalOptions, options);
}

$.ajaxSetup = ajaxSetup;

$.contains = contains;

const dataNS = '_mduiElementDataStorage';

/**
 * 在元素上设置键值对数据
 * @param element
 * @param object
 */
function setObjectToElement(element, object) {
    // @ts-ignore
    if (!element[dataNS]) {
        // @ts-ignore
        element[dataNS] = {};
    }
    each(object, (key, value) => {
        // @ts-ignore
        element[dataNS][toCamelCase(key)] = value;
    });
}
function data(element, key, value) {
    // 根据键值对设置值
    // data(element, { 'key' : 'value' })
    if (isObjectLike(key)) {
        setObjectToElement(element, key);
        return key;
    }
    // 根据 key、value 设置值
    // data(element, 'key', 'value')
    if (!isUndefined(value)) {
        setObjectToElement(element, { [key]: value });
        return value;
    }
    // 获取所有值
    // data(element)
    if (isUndefined(key)) {
        // @ts-ignore
        return element[dataNS] ? element[dataNS] : {};
    }
    // 从 dataNS 中获取指定值
    // data(element, 'key')
    key = toCamelCase(key);
    // @ts-ignore
    if (element[dataNS] && key in element[dataNS]) {
        // @ts-ignore
        return element[dataNS][key];
    }
    return undefined;
}

$.data = data;

$.each = each;

$.extend = function (...objectN) {
    if (objectN.length === 1) {
        each(objectN[0], (prop, value) => {
            this[prop] = value;
        });
        return this;
    }
    return extend(objectN.shift(), objectN.shift(), ...objectN);
};

function map(elements, callback) {
    let value;
    const ret = [];
    each(elements, (i, element) => {
        value = callback.call(window, element, i);
        if (value != null) {
            ret.push(value);
        }
    });
    return [].concat(...ret);
}

$.map = map;

$.merge = merge;

$.param = param;

/**
 * 移除指定元素上存放的数据
 * @param element 存放数据的元素
 * @param name
 * 数据键名
 *
 * 若未指定键名，将移除元素上所有数据
 *
 * 多个键名可以用空格分隔，或者用数组表示多个键名
  @example
```js
// 移除元素上键名为 name 的数据
removeData(document.body, 'name');
```
 * @example
```js
// 移除元素上键名为 name1 和 name2 的数据
removeData(document.body, 'name1 name2');
```
 * @example
```js
// 移除元素上键名为 name1 和 name2 的数据
removeData(document.body, ['name1', 'name2']);
```
 * @example
```js
// 移除元素上所有数据
removeData(document.body);
```
 */
function removeData(element, name) {
    // @ts-ignore
    if (!element[dataNS]) {
        return;
    }
    const remove = (nameItem) => {
        nameItem = toCamelCase(nameItem);
        // @ts-ignore
        if (element[dataNS][nameItem]) {
            // @ts-ignore
            element[dataNS][nameItem] = null;
            // @ts-ignore
            delete element[dataNS][nameItem];
        }
    };
    if (isUndefined(name)) {
        // @ts-ignore
        element[dataNS] = null;
        // @ts-ignore
        delete element[dataNS];
        // @ts-ignore
    }
    else if (isString(name)) {
        name
            .split(' ')
            .filter((nameItem) => nameItem)
            .forEach((nameItem) => remove(nameItem));
    }
    else {
        each(name, (_, nameItem) => remove(nameItem));
    }
}

$.removeData = removeData;

/**
 * 过滤掉数组中的重复元素
 * @param arr 数组
 * @example
```js
unique([1, 2, 12, 3, 2, 1, 2, 1, 1]);
// [1, 2, 12, 3]
```
 */
function unique(arr) {
    const result = [];
    each(arr, (_, val) => {
        if (result.indexOf(val) === -1) {
            result.push(val);
        }
    });
    return result;
}

$.unique = unique;

$.fn.add = function (selector) {
    return new JQ(unique(merge(this.get(), $(selector).get())));
};

each(['add', 'remove', 'toggle'], (_, name) => {
    $.fn[`${name}Class`] = function (className) {
        if (name === 'remove' && !arguments.length) {
            return this.each((_, element) => {
                element.setAttribute('class', '');
            });
        }
        return this.each((i, element) => {
            if (!isElement(element)) {
                return;
            }
            const classes = (isFunction(className)
                ? className.call(element, i, element.getAttribute('class') || '')
                : className)
                .split(' ')
                .filter((name) => name);
            each(classes, (_, cls) => {
                element.classList[name](cls);
            });
        });
    };
});

each(['insertBefore', 'insertAfter'], (nameIndex, name) => {
    $.fn[name] = function (target) {
        const $element = nameIndex ? $(this.get().reverse()) : this; // 顺序和 jQuery 保持一致
        const $target = $(target);
        const result = [];
        $target.each((index, target) => {
            if (!target.parentNode) {
                return;
            }
            $element.each((_, element) => {
                const newItem = index
                    ? element.cloneNode(true)
                    : element;
                const existingItem = nameIndex ? target.nextSibling : target;
                result.push(newItem);
                target.parentNode.insertBefore(newItem, existingItem);
            });
        });
        return $(nameIndex ? result.reverse() : result);
    };
});

/**
 * 是否不是 HTML 字符串（包裹在 <> 中）
 * @param target
 */
function isPlainText(target) {
    return (isString(target) && (target[0] !== '<' || target[target.length - 1] !== '>'));
}
each(['before', 'after'], (nameIndex, name) => {
    $.fn[name] = function (...args) {
        // after 方法，多个参数需要按参数顺序添加到元素后面，所以需要将参数顺序反向处理
        if (nameIndex === 1) {
            args = args.reverse();
        }
        return this.each((index, element) => {
            const targets = isFunction(args[0])
                ? [args[0].call(element, index, element.innerHTML)]
                : args;
            each(targets, (_, target) => {
                let $target;
                if (isPlainText(target)) {
                    $target = $(getChildNodesArray(target, 'div'));
                }
                else if (index && isElement(target)) {
                    $target = $(target.cloneNode(true));
                }
                else {
                    $target = $(target);
                }
                $target[nameIndex ? 'insertAfter' : 'insertBefore'](element);
            });
        });
    };
});

$.fn.off = function (types, selector, callback) {
    // types 是对象
    if (isObjectLike(types)) {
        each(types, (type, fn) => {
            // this.off('click', undefined, function () {})
            // this.off('click', '.box', function () {})
            this.off(type, selector, fn);
        });
        return this;
    }
    // selector 不存在
    if (selector === false || isFunction(selector)) {
        callback = selector;
        selector = undefined;
        // this.off('click', undefined, function () {})
    }
    // callback 传入 `false`，相当于 `return false`
    if (callback === false) {
        callback = returnFalse;
    }
    return this.each(function () {
        remove(this, types, callback, selector);
    });
};

$.fn.on = function (types, selector, data, callback, one) {
    // types 可以是 type/func 对象
    if (isObjectLike(types)) {
        // (types-Object, selector, data)
        if (!isString(selector)) {
            // (types-Object, data)
            data = data || selector;
            selector = undefined;
        }
        each(types, (type, fn) => {
            // selector 和 data 都可能是 undefined
            // @ts-ignore
            this.on(type, selector, data, fn, one);
        });
        return this;
    }
    if (data == null && callback == null) {
        // (types, fn)
        callback = selector;
        data = selector = undefined;
    }
    else if (callback == null) {
        if (isString(selector)) {
            // (types, selector, fn)
            callback = data;
            data = undefined;
        }
        else {
            // (types, data, fn)
            callback = data;
            data = selector;
            selector = undefined;
        }
    }
    if (callback === false) {
        callback = returnFalse;
    }
    else if (!callback) {
        return this;
    }
    // $().one()
    if (one) {
        // eslint-disable-next-line @typescript-eslint/no-this-alias
        const _this = this;
        const origCallback = callback;
        callback = function (event) {
            _this.off(event.type, selector, callback);
            // eslint-disable-next-line prefer-rest-params
            return origCallback.apply(this, arguments);
        };
    }
    return this.each(function () {
        add(this, types, callback, data, selector);
    });
};

each(ajaxEvents, (name, eventName) => {
    $.fn[name] = function (fn) {
        return this.on(eventName, (e, params) => {
            fn(e, params.xhr, params.options, params.data);
        });
    };
});

$.fn.map = function (callback) {
    return new JQ(map(this, (element, i) => callback.call(element, i, element)));
};

$.fn.clone = function () {
    return this.map(function () {
        return this.cloneNode(true);
    });
};

$.fn.is = function (selector) {
    let isMatched = false;
    if (isFunction(selector)) {
        this.each((index, element) => {
            if (selector.call(element, index, element)) {
                isMatched = true;
            }
        });
        return isMatched;
    }
    if (isString(selector)) {
        this.each((_, element) => {
            if (isDocument(element) || isWindow(element)) {
                return;
            }
            // @ts-ignore
            const matches = element.matches || element.msMatchesSelector;
            if (matches.call(element, selector)) {
                isMatched = true;
            }
        });
        return isMatched;
    }
    const $compareWith = $(selector);
    this.each((_, element) => {
        $compareWith.each((_, compare) => {
            if (element === compare) {
                isMatched = true;
            }
        });
    });
    return isMatched;
};

$.fn.remove = function (selector) {
    return this.each((_, element) => {
        if (element.parentNode && (!selector || $(element).is(selector))) {
            element.parentNode.removeChild(element);
        }
    });
};

each(['prepend', 'append'], (nameIndex, name) => {
    $.fn[name] = function (...args) {
        return this.each((index, element) => {
            const childNodes = element.childNodes;
            const childLength = childNodes.length;
            const child = childLength
                ? childNodes[nameIndex ? childLength - 1 : 0]
                : document.createElement('div');
            if (!childLength) {
                element.appendChild(child);
            }
            let contents = isFunction(args[0])
                ? [args[0].call(element, index, element.innerHTML)]
                : args;
            // 如果不是字符串，则仅第一个元素使用原始元素，其他的都克隆自第一个元素
            if (index) {
                contents = contents.map((content) => {
                    return isString(content) ? content : $(content).clone();
                });
            }
            $(child)[nameIndex ? 'after' : 'before'](...contents);
            if (!childLength) {
                element.removeChild(child);
            }
        });
    };
});

each(['appendTo', 'prependTo'], (nameIndex, name) => {
    $.fn[name] = function (target) {
        const extraChilds = [];
        const $target = $(target).map((_, element) => {
            const childNodes = element.childNodes;
            const childLength = childNodes.length;
            if (childLength) {
                return childNodes[nameIndex ? 0 : childLength - 1];
            }
            const child = document.createElement('div');
            element.appendChild(child);
            extraChilds.push(child);
            return child;
        });
        const $result = this[nameIndex ? 'insertBefore' : 'insertAfter']($target);
        $(extraChilds).remove();
        return $result;
    };
});

each(['attr', 'prop', 'css'], (nameIndex, name) => {
    function set(element, key, value) {
        // 值为 undefined 时，不修改
        if (isUndefined(value)) {
            return;
        }
        switch (nameIndex) {
            // attr
            case 0:
                if (isNull(value)) {
                    element.removeAttribute(key);
                }
                else {
                    element.setAttribute(key, value);
                }
                break;
            // prop
            case 1:
                // @ts-ignore
                element[key] = value;
                break;
            // css
            default:
                key = toCamelCase(key);
                // @ts-ignore
                element.style[key] = isNumber(value)
                    ? `${value}${cssNumber.indexOf(key) > -1 ? '' : 'px'}`
                    : value;
                break;
        }
    }
    function get(element, key) {
        switch (nameIndex) {
            // attr
            case 0:
                // 属性不存在时，原生 getAttribute 方法返回 null，而 jquery 返回 undefined。这里和 jquery 保持一致
                const value = element.getAttribute(key);
                return isNull(value) ? undefined : value;
            // prop
            case 1:
                // @ts-ignore
                return element[key];
            // css
            default:
                return getStyle(element, key);
        }
    }
    $.fn[name] = function (key, value) {
        if (isObjectLike(key)) {
            each(key, (k, v) => {
                // @ts-ignore
                this[name](k, v);
            });
            return this;
        }
        if (arguments.length === 1) {
            const element = this[0];
            return isElement(element) ? get(element, key) : undefined;
        }
        return this.each((i, element) => {
            set(element, key, isFunction(value) ? value.call(element, i, get(element, key)) : value);
        });
    };
});

$.fn.children = function (selector) {
    const children = [];
    this.each((_, element) => {
        each(element.childNodes, (__, childNode) => {
            if (!isElement(childNode)) {
                return;
            }
            if (!selector || $(childNode).is(selector)) {
                children.push(childNode);
            }
        });
    });
    return new JQ(unique(children));
};

$.fn.slice = function (...args) {
    return new JQ([].slice.apply(this, args));
};

$.fn.eq = function (index) {
    const ret = index === -1 ? this.slice(index) : this.slice(index, +index + 1);
    return new JQ(ret);
};

function dir($elements, nameIndex, node, selector, filter) {
    const ret = [];
    let target;
    $elements.each((_, element) => {
        target = element[node];
        // 不能包含最顶层的 document 元素
        while (target && isElement(target)) {
            // prevUntil, nextUntil, parentsUntil
            if (nameIndex === 2) {
                if (selector && $(target).is(selector)) {
                    break;
                }
                if (!filter || $(target).is(filter)) {
                    ret.push(target);
                }
            }
            // prev, next, parent
            else if (nameIndex === 0) {
                if (!selector || $(target).is(selector)) {
                    ret.push(target);
                }
                break;
            }
            // prevAll, nextAll, parents
            else {
                if (!selector || $(target).is(selector)) {
                    ret.push(target);
                }
            }
            // @ts-ignore
            target = target[node];
        }
    });
    return new JQ(unique(ret));
}

each(['', 's', 'sUntil'], (nameIndex, name) => {
    $.fn[`parent${name}`] = function (selector, filter) {
        // parents、parentsUntil 需要把元素的顺序反向处理，以便和 jQuery 的结果一致
        const $nodes = !nameIndex ? this : $(this.get().reverse());
        return dir($nodes, nameIndex, 'parentNode', selector, filter);
    };
});

$.fn.closest = function (selector) {
    if (this.is(selector)) {
        return this;
    }
    const matched = [];
    this.parents().each((_, element) => {
        if ($(element).is(selector)) {
            matched.push(element);
            return false;
        }
    });
    return new JQ(matched);
};

const rbrace = /^(?:{[\w\W]*\}|\[[\w\W]*\])$/;
// 从 `data-*` 中获取的值，需要经过该函数转换
function getData(value) {
    if (value === 'true') {
        return true;
    }
    if (value === 'false') {
        return false;
    }
    if (value === 'null') {
        return null;
    }
    if (value === +value + '') {
        return +value;
    }
    if (rbrace.test(value)) {
        return JSON.parse(value);
    }
    return value;
}
// 若 value 不存在，则从 `data-*` 中获取值
function dataAttr(element, key, value) {
    if (isUndefined(value) && element.nodeType === 1) {
        const name = 'data-' + toKebabCase(key);
        value = element.getAttribute(name);
        if (isString(value)) {
            try {
                value = getData(value);
            }
            catch (e) { }
        }
        else {
            value = undefined;
        }
    }
    return value;
}
$.fn.data = function (key, value) {
    // 获取所有值
    if (isUndefined(key)) {
        if (!this.length) {
            return undefined;
        }
        const element = this[0];
        const resultData = data(element);
        // window, document 上不存在 `data-*` 属性
        if (element.nodeType !== 1) {
            return resultData;
        }
        // 从 `data-*` 中获取值
        const attrs = element.attributes;
        let i = attrs.length;
        while (i--) {
            if (attrs[i]) {
                let name = attrs[i].name;
                if (name.indexOf('data-') === 0) {
                    name = toCamelCase(name.slice(5));
                    resultData[name] = dataAttr(element, name, resultData[name]);
                }
            }
        }
        return resultData;
    }
    // 同时设置多个值
    if (isObjectLike(key)) {
        return this.each(function () {
            data(this, key);
        });
    }
    // value 传入了 undefined
    if (arguments.length === 2 && isUndefined(value)) {
        return this;
    }
    // 设置值
    if (!isUndefined(value)) {
        return this.each(function () {
            data(this, key, value);
        });
    }
    // 获取值
    if (!this.length) {
        return undefined;
    }
    return dataAttr(this[0], key, data(this[0], key));
};

$.fn.empty = function () {
    return this.each(function () {
        this.innerHTML = '';
    });
};

$.fn.extend = function (obj) {
    each(obj, (prop, value) => {
        // 在 JQ 对象上扩展方法时，需要自己添加 typescript 的类型定义
        $.fn[prop] = value;
    });
    return this;
};

$.fn.filter = function (selector) {
    if (isFunction(selector)) {
        return this.map((index, element) => selector.call(element, index, element) ? element : undefined);
    }
    if (isString(selector)) {
        return this.map((_, element) => $(element).is(selector) ? element : undefined);
    }
    const $selector = $(selector);
    return this.map((_, element) => $selector.get().indexOf(element) > -1 ? element : undefined);
};

$.fn.first = function () {
    return this.eq(0);
};

$.fn.has = function (selector) {
    const $targets = isString(selector) ? this.find(selector) : $(selector);
    const { length } = $targets;
    return this.map(function () {
        for (let i = 0; i < length; i += 1) {
            if (contains(this, $targets[i])) {
                return this;
            }
        }
        return;
    });
};

$.fn.hasClass = function (className) {
    return this[0].classList.contains(className);
};

/**
 * 值上面的 padding、border、margin 处理
 * @param element
 * @param name
 * @param value
 * @param funcIndex
 * @param includeMargin
 * @param multiply
 */
function handleExtraWidth(element, name, value, funcIndex, includeMargin, multiply) {
    // 获取元素的 padding, border, margin 宽度（两侧宽度的和）
    const getExtraWidthValue = (extra) => {
        return (getExtraWidth(element, name.toLowerCase(), extra) *
            multiply);
    };
    if (funcIndex === 2 && includeMargin) {
        value += getExtraWidthValue('margin');
    }
    if (isBorderBox(element)) {
        // IE 为 box-sizing: border-box 时，得到的值不含 border 和 padding，这里先修复
        // 仅获取时需要处理，multiply === 1 为 get
        if (isIE() && multiply === 1) {
            value += getExtraWidthValue('border');
            value += getExtraWidthValue('padding');
        }
        if (funcIndex === 0) {
            value -= getExtraWidthValue('border');
        }
        if (funcIndex === 1) {
            value -= getExtraWidthValue('border');
            value -= getExtraWidthValue('padding');
        }
    }
    else {
        if (funcIndex === 0) {
            value += getExtraWidthValue('padding');
        }
        if (funcIndex === 2) {
            value += getExtraWidthValue('border');
            value += getExtraWidthValue('padding');
        }
    }
    return value;
}
/**
 * 获取元素的样式值
 * @param element
 * @param name
 * @param funcIndex 0: innerWidth, innerHeight; 1: width, height; 2: outerWidth, outerHeight
 * @param includeMargin
 */
function get(element, name, funcIndex, includeMargin) {
    const clientProp = `client${name}`;
    const scrollProp = `scroll${name}`;
    const offsetProp = `offset${name}`;
    const innerProp = `inner${name}`;
    // $(window).width()
    if (isWindow(element)) {
        // outerWidth, outerHeight 需要包含滚动条的宽度
        return funcIndex === 2
            ? element[innerProp]
            : toElement(document)[clientProp];
    }
    // $(document).width()
    if (isDocument(element)) {
        const doc = toElement(element);
        return Math.max(
        // @ts-ignore
        element.body[scrollProp], doc[scrollProp], 
        // @ts-ignore
        element.body[offsetProp], doc[offsetProp], doc[clientProp]);
    }
    const value = parseFloat(getComputedStyleValue(element, name.toLowerCase()) || '0');
    return handleExtraWidth(element, name, value, funcIndex, includeMargin, 1);
}
/**
 * 设置元素的样式值
 * @param element
 * @param elementIndex
 * @param name
 * @param funcIndex 0: innerWidth, innerHeight; 1: width, height; 2: outerWidth, outerHeight
 * @param includeMargin
 * @param value
 */
function set(element, elementIndex, name, funcIndex, includeMargin, value) {
    let computedValue = isFunction(value)
        ? value.call(element, elementIndex, get(element, name, funcIndex, includeMargin))
        : value;
    if (computedValue == null) {
        return;
    }
    const $element = $(element);
    const dimension = name.toLowerCase();
    // 特殊的值，不需要计算 padding、border、margin
    if (['auto', 'inherit', ''].indexOf(computedValue) > -1) {
        $element.css(dimension, computedValue);
        return;
    }
    // 其他值保留原始单位。注意：如果不使用 px 作为单位，则算出的值一般是不准确的
    const suffix = computedValue.toString().replace(/\b[0-9.]*/, '');
    const numerical = parseFloat(computedValue);
    computedValue =
        handleExtraWidth(element, name, numerical, funcIndex, includeMargin, -1) +
            (suffix || 'px');
    $element.css(dimension, computedValue);
}
each(['Width', 'Height'], (_, name) => {
    each([`inner${name}`, name.toLowerCase(), `outer${name}`], (funcIndex, funcName) => {
        $.fn[funcName] = function (margin, value) {
            // 是否是赋值操作
            const isSet = arguments.length && (funcIndex < 2 || !isBoolean(margin));
            const includeMargin = margin === true || value === true;
            // 获取第一个元素的值
            if (!isSet) {
                return this.length
                    ? get(this[0], name, funcIndex, includeMargin)
                    : undefined;
            }
            // 设置每个元素的值
            return this.each((index, element) => set(element, index, name, funcIndex, includeMargin, margin));
        };
    });
});

$.fn.hide = function () {
    return this.each(function () {
        this.style.display = 'none';
    });
};

each(['val', 'html', 'text'], (nameIndex, name) => {
    const props = {
        0: 'value',
        1: 'innerHTML',
        2: 'textContent',
    };
    const propName = props[nameIndex];
    function get($elements) {
        // text() 获取所有元素的文本
        if (nameIndex === 2) {
            // @ts-ignore
            return map($elements, (element) => toElement(element)[propName]).join('');
        }
        // 空集合时，val() 和 html() 返回 undefined
        if (!$elements.length) {
            return undefined;
        }
        // val() 和 html() 仅获取第一个元素的内容
        const firstElement = $elements[0];
        // select multiple 返回数组
        if (nameIndex === 0 && $(firstElement).is('select[multiple]')) {
            return map($(firstElement).find('option:checked'), (element) => element.value);
        }
        // @ts-ignore
        return firstElement[propName];
    }
    function set(element, value) {
        // text() 和 html() 赋值为 undefined，则保持原内容不变
        // val() 赋值为 undefined 则赋值为空
        if (isUndefined(value)) {
            if (nameIndex !== 0) {
                return;
            }
            value = '';
        }
        if (nameIndex === 1 && isElement(value)) {
            value = value.outerHTML;
        }
        // @ts-ignore
        element[propName] = value;
    }
    $.fn[name] = function (value) {
        // 获取值
        if (!arguments.length) {
            return get(this);
        }
        // 设置值
        return this.each((i, element) => {
            const computedValue = isFunction(value)
                ? value.call(element, i, get($(element)))
                : value;
            // value 是数组，则选中数组中的元素，反选不在数组中的元素
            if (nameIndex === 0 && Array.isArray(computedValue)) {
                // select[multiple]
                if ($(element).is('select[multiple]')) {
                    map($(element).find('option'), (option) => (option.selected =
                        computedValue.indexOf(option.value) >
                            -1));
                }
                // 其他 checkbox, radio 等元素
                else {
                    element.checked =
                        computedValue.indexOf(element.value) > -1;
                }
            }
            else {
                set(element, computedValue);
            }
        });
    };
});

$.fn.index = function (selector) {
    if (!arguments.length) {
        return this.eq(0).parent().children().get().indexOf(this[0]);
    }
    if (isString(selector)) {
        return $(selector).get().indexOf(this[0]);
    }
    return this.get().indexOf($(selector)[0]);
};

$.fn.last = function () {
    return this.eq(-1);
};

each(['', 'All', 'Until'], (nameIndex, name) => {
    $.fn[`next${name}`] = function (selector, filter) {
        return dir(this, nameIndex, 'nextElementSibling', selector, filter);
    };
});

$.fn.not = function (selector) {
    const $excludes = this.filter(selector);
    return this.map((_, element) => $excludes.index(element) > -1 ? undefined : element);
};

/**
 * 返回最近的用于定位的父元素
 */
$.fn.offsetParent = function () {
    return this.map(function () {
        let offsetParent = this.offsetParent;
        while (offsetParent && $(offsetParent).css('position') === 'static') {
            offsetParent = offsetParent.offsetParent;
        }
        return offsetParent || document.documentElement;
    });
};

function floatStyle($element, name) {
    return parseFloat($element.css(name));
}
$.fn.position = function () {
    if (!this.length) {
        return undefined;
    }
    const $element = this.eq(0);
    let currentOffset;
    let parentOffset = {
        left: 0,
        top: 0,
    };
    if ($element.css('position') === 'fixed') {
        currentOffset = $element[0].getBoundingClientRect();
    }
    else {
        currentOffset = $element.offset();
        const $offsetParent = $element.offsetParent();
        parentOffset = $offsetParent.offset();
        parentOffset.top += floatStyle($offsetParent, 'border-top-width');
        parentOffset.left += floatStyle($offsetParent, 'border-left-width');
    }
    return {
        top: currentOffset.top - parentOffset.top - floatStyle($element, 'margin-top'),
        left: currentOffset.left -
            parentOffset.left -
            floatStyle($element, 'margin-left'),
    };
};

function get$1(element) {
    if (!element.getClientRects().length) {
        return { top: 0, left: 0 };
    }
    const rect = element.getBoundingClientRect();
    const win = element.ownerDocument.defaultView;
    return {
        top: rect.top + win.pageYOffset,
        left: rect.left + win.pageXOffset,
    };
}
function set$1(element, value, index) {
    const $element = $(element);
    const position = $element.css('position');
    if (position === 'static') {
        $element.css('position', 'relative');
    }
    const currentOffset = get$1(element);
    const currentTopString = $element.css('top');
    const currentLeftString = $element.css('left');
    let currentTop;
    let currentLeft;
    const calculatePosition = (position === 'absolute' || position === 'fixed') &&
        (currentTopString + currentLeftString).indexOf('auto') > -1;
    if (calculatePosition) {
        const currentPosition = $element.position();
        currentTop = currentPosition.top;
        currentLeft = currentPosition.left;
    }
    else {
        currentTop = parseFloat(currentTopString);
        currentLeft = parseFloat(currentLeftString);
    }
    const computedValue = isFunction(value)
        ? value.call(element, index, extend({}, currentOffset))
        : value;
    $element.css({
        top: computedValue.top != null
            ? computedValue.top - currentOffset.top + currentTop
            : undefined,
        left: computedValue.left != null
            ? computedValue.left - currentOffset.left + currentLeft
            : undefined,
    });
}
$.fn.offset = function (value) {
    // 获取坐标
    if (!arguments.length) {
        if (!this.length) {
            return undefined;
        }
        return get$1(this[0]);
    }
    // 设置坐标
    return this.each(function (index) {
        set$1(this, value, index);
    });
};

$.fn.one = function (types, selector, data, callback) {
    // @ts-ignore
    return this.on(types, selector, data, callback, true);
};

each(['', 'All', 'Until'], (nameIndex, name) => {
    $.fn[`prev${name}`] = function (selector, filter) {
        // prevAll、prevUntil 需要把元素的顺序倒序处理，以便和 jQuery 的结果一致
        const $nodes = !nameIndex ? this : $(this.get().reverse());
        return dir($nodes, nameIndex, 'previousElementSibling', selector, filter);
    };
});

$.fn.removeAttr = function (attributeName) {
    const names = attributeName.split(' ').filter((name) => name);
    return this.each(function () {
        each(names, (_, name) => {
            this.removeAttribute(name);
        });
    });
};

$.fn.removeData = function (name) {
    return this.each(function () {
        removeData(this, name);
    });
};

$.fn.removeProp = function (name) {
    return this.each(function () {
        try {
            // @ts-ignore
            delete this[name];
        }
        catch (e) { }
    });
};

$.fn.replaceWith = function (newContent) {
    this.each((index, element) => {
        let content = newContent;
        if (isFunction(content)) {
            content = content.call(element, index, element.innerHTML);
        }
        else if (index && !isString(content)) {
            content = $(content).clone();
        }
        $(element).before(content);
    });
    return this.remove();
};

$.fn.replaceAll = function (target) {
    return $(target).map((index, element) => {
        $(element).replaceWith(index ? this.clone() : this);
        return this.get();
    });
};

/**
 * 将表单元素的值组合成键值对数组
 * @returns {Array}
 */
$.fn.serializeArray = function () {
    const result = [];
    this.each((_, element) => {
        const elements = element instanceof HTMLFormElement ? element.elements : [element];
        $(elements).each((_, element) => {
            const $element = $(element);
            const type = element.type;
            const nodeName = element.nodeName.toLowerCase();
            if (nodeName !== 'fieldset' &&
                element.name &&
                !element.disabled &&
                ['input', 'select', 'textarea', 'keygen'].indexOf(nodeName) > -1 &&
                ['submit', 'button', 'image', 'reset', 'file'].indexOf(type) === -1 &&
                (['radio', 'checkbox'].indexOf(type) === -1 ||
                    element.checked)) {
                const value = $element.val();
                const valueArr = Array.isArray(value) ? value : [value];
                valueArr.forEach((value) => {
                    result.push({
                        name: element.name,
                        value,
                    });
                });
            }
        });
    });
    return result;
};

$.fn.serialize = function () {
    return param(this.serializeArray());
};

const elementDisplay = {};
/**
 * 获取元素的初始 display 值，用于 .show() 方法
 * @param nodeName
 */
function defaultDisplay(nodeName) {
    let element;
    let display;
    if (!elementDisplay[nodeName]) {
        element = document.createElement(nodeName);
        document.body.appendChild(element);
        display = getStyle(element, 'display');
        element.parentNode.removeChild(element);
        if (display === 'none') {
            display = 'block';
        }
        elementDisplay[nodeName] = display;
    }
    return elementDisplay[nodeName];
}
/**
 * 显示指定元素
 * @returns {JQ}
 */
$.fn.show = function () {
    return this.each(function () {
        if (this.style.display === 'none') {
            this.style.display = '';
        }
        if (getStyle(this, 'display') === 'none') {
            this.style.display = defaultDisplay(this.nodeName);
        }
    });
};

/**
 * 取得同辈元素的集合
 * @param selector {String=}
 * @returns {JQ}
 */
$.fn.siblings = function (selector) {
    return this.prevAll(selector).add(this.nextAll(selector));
};

/**
 * 切换元素的显示状态
 */
$.fn.toggle = function () {
    return this.each(function () {
        getStyle(this, 'display') === 'none' ? $(this).show() : $(this).hide();
    });
};

$.fn.reflow = function () {
    return this.each(function () {
        return this.clientLeft;
    });
};

$.fn.transition = function (duration) {
    if (isNumber(duration)) {
        duration = `${duration}ms`;
    }
    return this.each(function () {
        this.style.webkitTransitionDuration = duration;
        this.style.transitionDuration = duration;
    });
};

$.fn.transitionEnd = function (callback) {
    // eslint-disable-next-line @typescript-eslint/no-this-alias
    const that = this;
    const events = ['webkitTransitionEnd', 'transitionend'];
    function fireCallback(e) {
        if (e.target !== this) {
            return;
        }
        // @ts-ignore
        callback.call(this, e);
        each(events, (_, event) => {
            that.off(event, fireCallback);
        });
    }
    each(events, (_, event) => {
        that.on(event, fireCallback);
    });
    return this;
};

$.fn.transformOrigin = function (transformOrigin) {
    return this.each(function () {
        this.style.webkitTransformOrigin = transformOrigin;
        this.style.transformOrigin = transformOrigin;
    });
};

$.fn.transform = function (transform) {
    return this.each(function () {
        this.style.webkitTransform = transform;
        this.style.transform = transform;
    });
};

/**
 * CSS 选择器和初始化函数组成的对象
 */
const entries = {};
/**
 * 注册并执行初始化函数
 * @param selector CSS 选择器
 * @param apiInit 初始化函数
 * @param i 元素索引
 * @param element 元素
 */
function mutation(selector, apiInit, i, element) {
    let selectors = data(element, '_mdui_mutation');
    if (!selectors) {
        selectors = [];
        data(element, '_mdui_mutation', selectors);
    }
    if (selectors.indexOf(selector) === -1) {
        selectors.push(selector);
        apiInit.call(element, i, element);
    }
}

$.fn.mutation = function () {
    return this.each((i, element) => {
        const $this = $(element);
        each(entries, (selector, apiInit) => {
            if ($this.is(selector)) {
                mutation(selector, apiInit, i, element);
            }
            $this.find(selector).each((i, element) => {
                mutation(selector, apiInit, i, element);
            });
        });
    });
};

$.showOverlay = function (zIndex) {
    let $overlay = $('.mdui-overlay');
    if ($overlay.length) {
        $overlay.data('_overlay_is_deleted', false);
        if (!isUndefined(zIndex)) {
            $overlay.css('z-index', zIndex);
        }
    }
    else {
        if (isUndefined(zIndex)) {
            zIndex = 2000;
        }
        $overlay = $('<div class="mdui-overlay">')
            .appendTo(document.body)
            .reflow()
            .css('z-index', zIndex);
    }
    let level = $overlay.data('_overlay_level') || 0;
    return $overlay.data('_overlay_level', ++level).addClass('mdui-overlay-show');
};

$.hideOverlay = function (force = false) {
    const $overlay = $('.mdui-overlay');
    if (!$overlay.length) {
        return;
    }
    let level = force ? 1 : $overlay.data('_overlay_level');
    if (level > 1) {
        $overlay.data('_overlay_level', --level);
        return;
    }
    $overlay
        .data('_overlay_level', 0)
        .removeClass('mdui-overlay-show')
        .data('_overlay_is_deleted', true)
        .transitionEnd(() => {
        if ($overlay.data('_overlay_is_deleted')) {
            $overlay.remove();
        }
    });
};

$.lockScreen = function () {
    const $body = $('body');
    // 不直接把 body 设为 box-sizing: border-box，避免污染全局样式
    const newBodyWidth = $body.width();
    let level = $body.data('_lockscreen_level') || 0;
    $body
        .addClass('mdui-locked')
        .width(newBodyWidth)
        .data('_lockscreen_level', ++level);
};

$.unlockScreen = function (force = false) {
    const $body = $('body');
    let level = force ? 1 : $body.data('_lockscreen_level');
    if (level > 1) {
        $body.data('_lockscreen_level', --level);
        return;
    }
    $body.data('_lockscreen_level', 0).removeClass('mdui-locked').width('');
};

$.throttle = function (fn, delay = 16) {
    let timer = null;
    return function (...args) {
        if (isNull(timer)) {
            timer = setTimeout(() => {
                fn.apply(this, args);
                timer = null;
            }, delay);
        }
    };
};

const GUID = {};
$.guid = function (name) {
    if (!isUndefined(name) && !isUndefined(GUID[name])) {
        return GUID[name];
    }
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    const guid = '_' +
        s4() +
        s4() +
        '-' +
        s4() +
        '-' +
        s4() +
        '-' +
        s4() +
        '-' +
        s4() +
        s4() +
        s4();
    if (!isUndefined(name)) {
        GUID[name] = guid;
    }
    return guid;
};

mdui.mutation = function (selector, apiInit) {
    if (isUndefined(selector) || isUndefined(apiInit)) {
        $(document).mutation();
        return;
    }
    entries[selector] = apiInit;
    $(selector).each((i, element) => mutation(selector, apiInit, i, element));
};

/**
 * 触发组件上的事件
 * @param eventName 事件名
 * @param componentName 组件名
 * @param target 在该元素上触发事件
 * @param instance 组件实例
 * @param parameters 事件参数
 */
function componentEvent(eventName, componentName, target, instance, parameters) {
    if (!parameters) {
        parameters = {};
    }
    // @ts-ignore
    parameters.inst = instance;
    const fullEventName = `${eventName}.mdui.${componentName}`;
    // jQuery 事件
    // @ts-ignore
    if (typeof jQuery !== 'undefined') {
        // @ts-ignore
        jQuery(target).trigger(fullEventName, parameters);
    }
    const $target = $(target);
    // mdui.jq 事件
    $target.trigger(fullEventName, parameters);
    const eventParams = {
        bubbles: true,
        cancelable: true,
        detail: parameters,
    };
    const eventObject = new CustomEvent(fullEventName, eventParams);
    // @ts-ignore
    eventObject._detail = parameters;
    $target[0].dispatchEvent(eventObject);
}

const $document = $(document);
const $window = $(window);
$('body');

const DEFAULT_OPTIONS = {
    tolerance: 5,
    offset: 0,
    initialClass: 'mdui-headroom',
    pinnedClass: 'mdui-headroom-pinned-top',
    unpinnedClass: 'mdui-headroom-unpinned-top',
};
class Headroom {
    constructor(selector, options = {}) {
        /**
         * 配置参数
         */
        this.options = extend({}, DEFAULT_OPTIONS);
        /**
         * 当前 headroom 的状态
         */
        this.state = 'pinned';
        /**
         * 当前是否启用
         */
        this.isEnable = false;
        /**
         * 上次滚动后，垂直方向的距离
         */
        this.lastScrollY = 0;
        /**
         * AnimationFrame ID
         */
        this.rafId = 0;
        this.$element = $(selector).first();
        extend(this.options, options);
        // tolerance 参数若为数值，转换为对象
        const tolerance = this.options.tolerance;
        if (isNumber(tolerance)) {
            this.options.tolerance = {
                down: tolerance,
                up: tolerance,
            };
        }
        this.enable();
    }
    /**
     * 滚动时的处理
     */
    onScroll() {
        this.rafId = window.requestAnimationFrame(() => {
            const currentScrollY = window.pageYOffset;
            const direction = currentScrollY > this.lastScrollY ? 'down' : 'up';
            const tolerance = this.options.tolerance[direction];
            const scrolled = Math.abs(currentScrollY - this.lastScrollY);
            const toleranceExceeded = scrolled >= tolerance;
            if (currentScrollY > this.lastScrollY &&
                currentScrollY >= this.options.offset &&
                toleranceExceeded) {
                this.unpin();
            }
            else if ((currentScrollY < this.lastScrollY && toleranceExceeded) ||
                currentScrollY <= this.options.offset) {
                this.pin();
            }
            this.lastScrollY = currentScrollY;
        });
    }
    /**
     * 触发组件事件
     * @param name
     */
    triggerEvent(name) {
        componentEvent(name, 'headroom', this.$element, this);
    }
    /**
     * 动画结束的回调
     */
    transitionEnd() {
        if (this.state === 'pinning') {
            this.state = 'pinned';
            this.triggerEvent('pinned');
        }
        if (this.state === 'unpinning') {
            this.state = 'unpinned';
            this.triggerEvent('unpinned');
        }
    }
    /**
     * 使元素固定住
     */
    pin() {
        if (this.state === 'pinning' ||
            this.state === 'pinned' ||
            !this.$element.hasClass(this.options.initialClass)) {
            return;
        }
        this.triggerEvent('pin');
        this.state = 'pinning';
        this.$element
            .removeClass(this.options.unpinnedClass)
            .addClass(this.options.pinnedClass)
            .transitionEnd(() => this.transitionEnd());
    }
    /**
     * 使元素隐藏
     */
    unpin() {
        if (this.state === 'unpinning' ||
            this.state === 'unpinned' ||
            !this.$element.hasClass(this.options.initialClass)) {
            return;
        }
        this.triggerEvent('unpin');
        this.state = 'unpinning';
        this.$element
            .removeClass(this.options.pinnedClass)
            .addClass(this.options.unpinnedClass)
            .transitionEnd(() => this.transitionEnd());
    }
    /**
     * 启用 headroom 插件
     */
    enable() {
        if (this.isEnable) {
            return;
        }
        this.isEnable = true;
        this.state = 'pinned';
        this.$element
            .addClass(this.options.initialClass)
            .removeClass(this.options.pinnedClass)
            .removeClass(this.options.unpinnedClass);
        this.lastScrollY = window.pageYOffset;
        $window.on('scroll', () => this.onScroll());
    }
    /**
     * 禁用 headroom 插件
     */
    disable() {
        if (!this.isEnable) {
            return;
        }
        this.isEnable = false;
        this.$element
            .removeClass(this.options.initialClass)
            .removeClass(this.options.pinnedClass)
            .removeClass(this.options.unpinnedClass);
        $window.off('scroll', () => this.onScroll());
        window.cancelAnimationFrame(this.rafId);
    }
    /**
     * 获取当前状态。共包含四种状态：`pinning`、`pinned`、`unpinning`、`unpinned`
     */
    getState() {
        return this.state;
    }
}
mdui.Headroom = Headroom;

/**
 * 解析 DATA API 参数
 * @param element 元素
 * @param name 属性名
 */
function parseOptions(element, name) {
    const attr = $(element).attr(name);
    if (!attr) {
        return {};
    }
    return new Function('', `var json = ${attr}; return JSON.parse(JSON.stringify(json));`)();
}

const customAttr = 'mdui-headroom';
$(() => {
    mdui.mutation(`[${customAttr}]`, function () {
        new mdui.Headroom(this, parseOptions(this, customAttr));
    });
});

const DEFAULT_OPTIONS$1 = {
    accordion: false,
};
class CollapseAbstract {
    constructor(selector, options = {}) {
        /**
         * 配置参数
         */
        this.options = extend({}, DEFAULT_OPTIONS$1);
        // CSS 类名
        const classPrefix = `mdui-${this.getNamespace()}-item`;
        this.classItem = classPrefix;
        this.classItemOpen = `${classPrefix}-open`;
        this.classHeader = `${classPrefix}-header`;
        this.classBody = `${classPrefix}-body`;
        this.$element = $(selector).first();
        extend(this.options, options);
        this.bindEvent();
    }
    /**
     * 绑定事件
     */
    bindEvent() {
        // eslint-disable-next-line @typescript-eslint/no-this-alias
        const that = this;
        // 点击 header 时，打开/关闭 item
        this.$element.on('click', `.${this.classHeader}`, function () {
            const $header = $(this);
            const $item = $header.parent();
            const $items = that.getItems();
            $items.each((_, item) => {
                if ($item.is(item)) {
                    that.toggle(item);
                }
            });
        });
        // 点击关闭按钮时，关闭 item
        this.$element.on('click', `[mdui-${this.getNamespace()}-item-close]`, function () {
            const $target = $(this);
            const $item = $target.parents(`.${that.classItem}`).first();
            that.close($item);
        });
    }
    /**
     * 指定 item 是否处于打开状态
     * @param $item
     */
    isOpen($item) {
        return $item.hasClass(this.classItemOpen);
    }
    /**
     * 获取所有 item
     */
    getItems() {
        return this.$element.children(`.${this.classItem}`);
    }
    /**
     * 获取指定 item
     * @param item
     */
    getItem(item) {
        if (isNumber(item)) {
            return this.getItems().eq(item);
        }
        return $(item).first();
    }
    /**
     * 触发组件事件
     * @param name 事件名
     * @param $item 事件触发的目标 item
     */
    triggerEvent(name, $item) {
        componentEvent(name, this.getNamespace(), $item, this);
    }
    /**
     * 动画结束回调
     * @param $content body 元素
     * @param $item item 元素
     */
    transitionEnd($content, $item) {
        if (this.isOpen($item)) {
            $content.transition(0).height('auto').reflow().transition('');
            this.triggerEvent('opened', $item);
        }
        else {
            $content.height('');
            this.triggerEvent('closed', $item);
        }
    }
    /**
     * 打开指定面板项
     * @param item 面板项的索引号、或 CSS 选择器、或 DOM 元素、或 JQ 对象
     */
    open(item) {
        const $item = this.getItem(item);
        if (this.isOpen($item)) {
            return;
        }
        // 关闭其他项
        if (this.options.accordion) {
            this.$element.children(`.${this.classItemOpen}`).each((_, element) => {
                const $element = $(element);
                if (!$element.is($item)) {
                    this.close($element);
                }
            });
        }
        const $content = $item.children(`.${this.classBody}`);
        $content
            .height($content[0].scrollHeight)
            .transitionEnd(() => this.transitionEnd($content, $item));
        this.triggerEvent('open', $item);
        $item.addClass(this.classItemOpen);
    }
    /**
     * 关闭指定面板项
     * @param item 面板项的索引号、或 CSS 选择器、或 DOM 元素、或 JQ 对象
     */
    close(item) {
        const $item = this.getItem(item);
        if (!this.isOpen($item)) {
            return;
        }
        const $content = $item.children(`.${this.classBody}`);
        this.triggerEvent('close', $item);
        $item.removeClass(this.classItemOpen);
        $content
            .transition(0)
            .height($content[0].scrollHeight)
            .reflow()
            .transition('')
            .height('')
            .transitionEnd(() => this.transitionEnd($content, $item));
    }
    /**
     * 切换指定面板项的打开状态
     * @param item 面板项的索引号、或 CSS 选择器、或 DOM 元素、或 JQ 对象
     */
    toggle(item) {
        const $item = this.getItem(item);
        this.isOpen($item) ? this.close($item) : this.open($item);
    }
    /**
     * 打开所有面板项
     */
    openAll() {
        this.getItems().each((_, element) => this.open(element));
    }
    /**
     * 关闭所有面板项
     */
    closeAll() {
        this.getItems().each((_, element) => this.close(element));
    }
}

class Collapse extends CollapseAbstract {
    getNamespace() {
        return 'collapse';
    }
}
mdui.Collapse = Collapse;

const customAttr$1 = 'mdui-collapse';
$(() => {
    mdui.mutation(`[${customAttr$1}]`, function () {
        new mdui.Collapse(this, parseOptions(this, customAttr$1));
    });
});

class Panel extends CollapseAbstract {
    getNamespace() {
        return 'panel';
    }
}
mdui.Panel = Panel;

const customAttr$2 = 'mdui-panel';
$(() => {
    mdui.mutation(`[${customAttr$2}]`, function () {
        new mdui.Panel(this, parseOptions(this, customAttr$2));
    });
});

/**
 * touch 事件后的 500ms 内禁用 mousedown 事件
 *
 * 不支持触控的屏幕上事件顺序为 mousedown -> mouseup -> click
 * 支持触控的屏幕上事件顺序为 touchstart -> touchend -> mousedown -> mouseup -> click
 *
 * 在每一个事件中都使用 TouchHandler.isAllow(event) 判断事件是否可执行
 * 在 touchstart 和 touchmove、touchend、touchcancel
 *
 * (function () {
 *   $document
 *     .on(start, function (e) {
 *       if (!isAllow(e)) {
 *         return;
 *       }
 *       register(e);
 *       console.log(e.type);
 *     })
 *     .on(move, function (e) {
 *       if (!isAllow(e)) {
 *         return;
 *       }
 *       console.log(e.type);
 *     })
 *     .on(end, function (e) {
 *       if (!isAllow(e)) {
 *         return;
 *       }
 *       console.log(e.type);
 *     })
 *     .on(unlock, register);
 * })();
 */
const startEvent = 'touchstart mousedown';
const moveEvent = 'touchmove mousemove';
const endEvent = 'touchend mouseup';
const cancelEvent = 'touchcancel mouseleave';
const unlockEvent = 'touchend touchmove touchcancel';
let touches = 0;
/**
 * 该事件是否被允许，在执行事件前调用该方法判断事件是否可以执行
 * 若已触发 touch 事件，则阻止之后的鼠标事件
 * @param event
 */
function isAllow(event) {
    return !(touches &&
        [
            'mousedown',
            'mouseup',
            'mousemove',
            'click',
            'mouseover',
            'mouseout',
            'mouseenter',
            'mouseleave',
        ].indexOf(event.type) > -1);
}
/**
 * 在 touchstart 和 touchmove、touchend、touchcancel 事件中调用该方法注册事件
 * @param event
 */
function register(event) {
    if (event.type === 'touchstart') {
        // 触发了 touch 事件
        touches += 1;
    }
    else if (['touchmove', 'touchend', 'touchcancel'].indexOf(event.type) > -1) {
        // touch 事件结束 500ms 后解除对鼠标事件的阻止
        setTimeout(function () {
            if (touches) {
                touches -= 1;
            }
        }, 500);
    }
}

/**
 * Inspired by https://github.com/nolimits4web/Framework7/blob/master/src/js/fast-clicks.js
 * https://github.com/nolimits4web/Framework7/blob/master/LICENSE
 *
 * Inspired by https://github.com/fians/Waves
 */
/**
 * 显示涟漪动画
 * @param event
 * @param $ripple
 */
function show(event, $ripple) {
    // 鼠标右键不产生涟漪
    if (event instanceof MouseEvent && event.button === 2) {
        return;
    }
    // 点击位置坐标
    const touchPosition = typeof TouchEvent !== 'undefined' &&
        event instanceof TouchEvent &&
        event.touches.length
        ? event.touches[0]
        : event;
    const touchStartX = touchPosition.pageX;
    const touchStartY = touchPosition.pageY;
    // 涟漪位置
    const offset = $ripple.offset();
    const height = $ripple.innerHeight();
    const width = $ripple.innerWidth();
    const center = {
        x: touchStartX - offset.left,
        y: touchStartY - offset.top,
    };
    const diameter = Math.max(Math.pow(Math.pow(height, 2) + Math.pow(width, 2), 0.5), 48);
    // 涟漪扩散动画
    const translate = `translate3d(${-center.x + width / 2}px,` +
        `${-center.y + height / 2}px, 0) scale(1)`;
    // 涟漪的 DOM 结构，并缓存动画效果
    $(`<div class="mdui-ripple-wave" ` +
        `style="width:${diameter}px;height:${diameter}px;` +
        `margin-top:-${diameter / 2}px;margin-left:-${diameter / 2}px;` +
        `left:${center.x}px;top:${center.y}px;"></div>`)
        .data('_ripple_wave_translate', translate)
        .prependTo($ripple)
        .reflow()
        .transform(translate);
}
/**
 * 隐藏并移除涟漪
 * @param $wave
 */
function removeRipple($wave) {
    if (!$wave.length || $wave.data('_ripple_wave_removed')) {
        return;
    }
    $wave.data('_ripple_wave_removed', true);
    let removeTimer = setTimeout(() => $wave.remove(), 400);
    const translate = $wave.data('_ripple_wave_translate');
    $wave
        .addClass('mdui-ripple-wave-fill')
        .transform(translate.replace('scale(1)', 'scale(1.01)'))
        .transitionEnd(() => {
        clearTimeout(removeTimer);
        $wave
            .addClass('mdui-ripple-wave-out')
            .transform(translate.replace('scale(1)', 'scale(1.01)'));
        removeTimer = setTimeout(() => $wave.remove(), 700);
        setTimeout(() => {
            $wave.transitionEnd(() => {
                clearTimeout(removeTimer);
                $wave.remove();
            });
        }, 0);
    });
}
/**
 * 隐藏涟漪动画
 * @param this
 */
function hide() {
    const $ripple = $(this);
    $ripple.children('.mdui-ripple-wave').each((_, wave) => {
        removeRipple($(wave));
    });
    $ripple.off(`${moveEvent} ${endEvent} ${cancelEvent}`, hide);
}
/**
 * 显示涟漪，并绑定 touchend 等事件
 * @param event
 */
function showRipple(event) {
    if (!isAllow(event)) {
        return;
    }
    register(event);
    // Chrome 59 点击滚动条时，会在 document 上触发事件
    if (event.target === document) {
        return;
    }
    const $target = $(event.target);
    // 获取含 .mdui-ripple 类的元素
    const $ripple = $target.hasClass('mdui-ripple')
        ? $target
        : $target.parents('.mdui-ripple').first();
    if (!$ripple.length) {
        return;
    }
    // 禁用状态的元素上不产生涟漪效果
    if ($ripple.prop('disabled') || !isUndefined($ripple.attr('disabled'))) {
        return;
    }
    if (event.type === 'touchstart') {
        let hidden = false;
        // touchstart 触发指定时间后开始涟漪动画，避免手指滑动时也触发涟漪
        let timer = setTimeout(() => {
            timer = 0;
            show(event, $ripple);
        }, 200);
        const hideRipple = () => {
            // 如果手指没有移动，且涟漪动画还没有开始，则开始涟漪动画
            if (timer) {
                clearTimeout(timer);
                timer = 0;
                show(event, $ripple);
            }
            if (!hidden) {
                hidden = true;
                hide.call($ripple);
            }
        };
        // 手指移动后，移除涟漪动画
        const touchMove = () => {
            if (timer) {
                clearTimeout(timer);
                timer = 0;
            }
            hideRipple();
        };
        $ripple.on('touchmove', touchMove).on('touchend touchcancel', hideRipple);
    }
    else {
        show(event, $ripple);
        $ripple.on(`${moveEvent} ${endEvent} ${cancelEvent}`, hide);
    }
}
$(() => {
    $document.on(startEvent, showRipple).on(unlockEvent, register);
});

/**
 * 滑块的值改变后修改滑块样式
 * @param $slider
 */
function updateValueStyle($slider) {
    const data = $slider.data();
    const $track = data._slider_$track;
    const $fill = data._slider_$fill;
    const $thumb = data._slider_$thumb;
    const $input = data._slider_$input;
    const min = data._slider_min;
    const max = data._slider_max;
    const isDisabled = data._slider_disabled;
    const isDiscrete = data._slider_discrete;
    const $thumbText = data._slider_$thumbText;
    const value = $input.val();
    const percent = ((value - min) / (max - min)) * 100;
    $fill.width(`${percent}%`);
    $track.width(`${100 - percent}%`);
    if (isDisabled) {
        $fill.css('padding-right', '6px');
        $track.css('padding-left', '6px');
    }
    $thumb.css('left', `${percent}%`);
    if (isDiscrete) {
        $thumbText.text(value);
    }
    percent === 0
        ? $slider.addClass('mdui-slider-zero')
        : $slider.removeClass('mdui-slider-zero');
}
/**
 * 重新初始化滑块
 * @param $slider
 */
function reInit($slider) {
    const $track = $('<div class="mdui-slider-track"></div>');
    const $fill = $('<div class="mdui-slider-fill"></div>');
    const $thumb = $('<div class="mdui-slider-thumb"></div>');
    const $input = $slider.find('input[type="range"]');
    const isDisabled = $input[0].disabled;
    const isDiscrete = $slider.hasClass('mdui-slider-discrete');
    // 禁用状态
    isDisabled
        ? $slider.addClass('mdui-slider-disabled')
        : $slider.removeClass('mdui-slider-disabled');
    // 重新填充 HTML
    $slider.find('.mdui-slider-track').remove();
    $slider.find('.mdui-slider-fill').remove();
    $slider.find('.mdui-slider-thumb').remove();
    $slider.append($track).append($fill).append($thumb);
    // 间续型滑块
    let $thumbText = $();
    if (isDiscrete) {
        $thumbText = $('<span></span>');
        $thumb.empty().append($thumbText);
    }
    $slider.data('_slider_$track', $track);
    $slider.data('_slider_$fill', $fill);
    $slider.data('_slider_$thumb', $thumb);
    $slider.data('_slider_$input', $input);
    $slider.data('_slider_min', $input.attr('min'));
    $slider.data('_slider_max', $input.attr('max'));
    $slider.data('_slider_disabled', isDisabled);
    $slider.data('_slider_discrete', isDiscrete);
    $slider.data('_slider_$thumbText', $thumbText);
    // 设置默认值
    updateValueStyle($slider);
}
const rangeSelector = '.mdui-slider input[type="range"]';
$(() => {
    // 滑块滑动事件
    $document.on('input change', rangeSelector, function () {
        const $slider = $(this).parent();
        updateValueStyle($slider);
    });
    // 开始触摸滑块事件
    $document.on(startEvent, rangeSelector, function (event) {
        if (!isAllow(event)) {
            return;
        }
        register(event);
        if (this.disabled) {
            return;
        }
        const $slider = $(this).parent();
        $slider.addClass('mdui-slider-focus');
    });
    // 结束触摸滑块事件
    $document.on(endEvent, rangeSelector, function (event) {
        if (!isAllow(event)) {
            return;
        }
        if (this.disabled) {
            return;
        }
        const $slider = $(this).parent();
        $slider.removeClass('mdui-slider-focus');
    });
    $document.on(unlockEvent, rangeSelector, register);
    /**
     * 初始化滑块
     */
    mdui.mutation('.mdui-slider', function () {
        reInit($(this));
    });
});
mdui.updateSliders = function (selector) {
    const $elements = isUndefined(selector) ? $('.mdui-slider') : $(selector);
    $elements.each((_, element) => {
        reInit($(element));
    });
};

const DEFAULT_OPTIONS$2 = {
    trigger: 'click',
    loop: false,
};
class Tab {
    constructor(selector, options = {}) {
        /**
         * 配置参数
         */
        this.options = extend({}, DEFAULT_OPTIONS$2);
        /**
         * 当前激活的 tab 的索引号。为 -1 时表示没有激活的选项卡，或不存在选项卡
         */
        this.activeIndex = -1;
        this.$element = $(selector).first();
        extend(this.options, options);
        this.$tabs = this.$element.children('a');
        this.$indicator = $('<div class="mdui-tab-indicator"></div>').appendTo(this.$element);
        // 根据 url hash 获取默认激活的选项卡
        const hash = window.location.hash;
        if (hash) {
            this.$tabs.each((index, tab) => {
                if ($(tab).attr('href') === hash) {
                    this.activeIndex = index;
                    return false;
                }
                return true;
            });
        }
        // 含 .mdui-tab-active 的元素默认激活
        if (this.activeIndex === -1) {
            this.$tabs.each((index, tab) => {
                if ($(tab).hasClass('mdui-tab-active')) {
                    this.activeIndex = index;
                    return false;
                }
                return true;
            });
        }
        // 存在选项卡时，默认激活第一个选项卡
        if (this.$tabs.length && this.activeIndex === -1) {
            this.activeIndex = 0;
        }
        // 设置激活状态选项卡
        this.setActive();
        // 监听窗口大小变化事件，调整指示器位置
        $window.on('resize', $.throttle(() => this.setIndicatorPosition(), 100));
        // 监听点击选项卡事件
        this.$tabs.each((_, tab) => {
            this.bindTabEvent(tab);
        });
    }
    /**
     * 指定选项卡是否已禁用
     * @param $tab
     */
    isDisabled($tab) {
        return $tab.attr('disabled') !== undefined;
    }
    /**
     * 绑定在 Tab 上点击或悬浮的事件
     * @param tab
     */
    bindTabEvent(tab) {
        const $tab = $(tab);
        // 点击或鼠标移入触发的事件
        const clickEvent = () => {
            // 禁用状态的选项卡无法选中
            if (this.isDisabled($tab)) {
                return false;
            }
            this.activeIndex = this.$tabs.index(tab);
            this.setActive();
        };
        // 无论 trigger 是 click 还是 hover，都会响应 click 事件
        $tab.on('click', clickEvent);
        // trigger 为 hover 时，额外响应 mouseenter 事件
        if (this.options.trigger === 'hover') {
            $tab.on('mouseenter', clickEvent);
        }
        // 阻止链接的默认点击动作
        $tab.on('click', () => {
            if (($tab.attr('href') || '').indexOf('#') === 0) {
                return false;
            }
        });
    }
    /**
     * 触发组件事件
     * @param name
     * @param $element
     * @param parameters
     */
    triggerEvent(name, $element, parameters = {}) {
        componentEvent(name, 'tab', $element, this, parameters);
    }
    /**
     * 设置激活状态的选项卡
     */
    setActive() {
        this.$tabs.each((index, tab) => {
            const $tab = $(tab);
            const targetId = $tab.attr('href') || '';
            // 设置选项卡激活状态
            if (index === this.activeIndex && !this.isDisabled($tab)) {
                if (!$tab.hasClass('mdui-tab-active')) {
                    this.triggerEvent('change', this.$element, {
                        index: this.activeIndex,
                        id: targetId.substr(1),
                    });
                    this.triggerEvent('show', $tab);
                    $tab.addClass('mdui-tab-active');
                }
                $(targetId).show();
                this.setIndicatorPosition();
            }
            else {
                $tab.removeClass('mdui-tab-active');
                $(targetId).hide();
            }
        });
    }
    /**
     * 设置选项卡指示器的位置
     */
    setIndicatorPosition() {
        // 选项卡数量为 0 时，不显示指示器
        if (this.activeIndex === -1) {
            this.$indicator.css({
                left: 0,
                width: 0,
            });
            return;
        }
        const $activeTab = this.$tabs.eq(this.activeIndex);
        if (this.isDisabled($activeTab)) {
            return;
        }
        const activeTabOffset = $activeTab.offset();
        this.$indicator.css({
            left: `${activeTabOffset.left +
                this.$element[0].scrollLeft -
                this.$element[0].getBoundingClientRect().left}px`,
            width: `${$activeTab.innerWidth()}px`,
        });
    }
    /**
     * 切换到下一个选项卡
     */
    next() {
        if (this.activeIndex === -1) {
            return;
        }
        if (this.$tabs.length > this.activeIndex + 1) {
            this.activeIndex++;
        }
        else if (this.options.loop) {
            this.activeIndex = 0;
        }
        this.setActive();
    }
    /**
     * 切换到上一个选项卡
     */
    prev() {
        if (this.activeIndex === -1) {
            return;
        }
        if (this.activeIndex > 0) {
            this.activeIndex--;
        }
        else if (this.options.loop) {
            this.activeIndex = this.$tabs.length - 1;
        }
        this.setActive();
    }
    /**
     * 显示指定索引号、或指定id的选项卡
     * @param index 索引号、或id
     */
    show(index) {
        if (this.activeIndex === -1) {
            return;
        }
        if (isNumber(index)) {
            this.activeIndex = index;
        }
        else {
            this.$tabs.each((i, tab) => {
                if (tab.id === index) {
                    this.activeIndex = i;
                    return false;
                }
            });
        }
        this.setActive();
    }
    /**
     * 在父元素的宽度变化时，需要调用该方法重新调整指示器位置
     * 在添加或删除选项卡时，需要调用该方法
     */
    handleUpdate() {
        const $oldTabs = this.$tabs; // 旧的 tabs JQ对象
        const $newTabs = this.$element.children('a'); // 新的 tabs JQ对象
        const oldTabsElement = $oldTabs.get(); // 旧的 tabs 元素数组
        const newTabsElement = $newTabs.get(); // 新的 tabs 元素数组
        if (!$newTabs.length) {
            this.activeIndex = -1;
            this.$tabs = $newTabs;
            this.setIndicatorPosition();
            return;
        }
        // 重新遍历选项卡，找出新增的选项卡
        $newTabs.each((index, tab) => {
            // 有新增的选项卡
            if (oldTabsElement.indexOf(tab) < 0) {
                this.bindTabEvent(tab);
                if (this.activeIndex === -1) {
                    this.activeIndex = 0;
                }
                else if (index <= this.activeIndex) {
                    this.activeIndex++;
                }
            }
        });
        // 找出被移除的选项卡
        $oldTabs.each((index, tab) => {
            // 有被移除的选项卡
            if (newTabsElement.indexOf(tab) < 0) {
                if (index < this.activeIndex) {
                    this.activeIndex--;
                }
                else if (index === this.activeIndex) {
                    this.activeIndex = 0;
                }
            }
        });
        this.$tabs = $newTabs;
        this.setActive();
    }
}
mdui.Tab = Tab;

const customAttr$3 = 'mdui-tab';
$(() => {
    mdui.mutation(`[${customAttr$3}]`, function () {
        new mdui.Tab(this, parseOptions(this, customAttr$3));
    });
});

const DEFAULT_OPTIONS$3 = {
    position: 'auto',
    delay: 0,
    content: '',
};
class Tooltip {
    constructor(selector, options = {}) {
        /**
         * 配置参数
         */
        this.options = extend({}, DEFAULT_OPTIONS$3);
        /**
         * 当前 tooltip 的状态
         */
        this.state = 'closed';
        /**
         * setTimeout 的返回值
         */
        this.timeoutId = null;
        this.$target = $(selector).first();
        extend(this.options, options);
        // 创建 Tooltip HTML
        this.$element = $(`<div class="mdui-tooltip" id="${$.guid()}">${this.options.content}</div>`).appendTo(document.body);
        // 绑定事件。元素处于 disabled 状态时无法触发鼠标事件，为了统一，把 touch 事件也禁用
        // eslint-disable-next-line @typescript-eslint/no-this-alias
        const that = this;
        this.$target
            .on('touchstart mouseenter', function (event) {
            if (that.isDisabled(this)) {
                return;
            }
            if (!isAllow(event)) {
                return;
            }
            register(event);
            that.open();
        })
            .on('touchend mouseleave', function (event) {
            if (that.isDisabled(this)) {
                return;
            }
            if (!isAllow(event)) {
                return;
            }
            that.close();
        })
            .on(unlockEvent, function (event) {
            if (that.isDisabled(this)) {
                return;
            }
            register(event);
        });
    }
    /**
     * 元素是否已禁用
     * @param element
     */
    isDisabled(element) {
        return (element.disabled ||
            $(element).attr('disabled') !== undefined);
    }
    /**
     * 是否是桌面设备
     */
    isDesktop() {
        return $window.width() > 1024;
    }
    /**
     * 设置 Tooltip 的位置
     */
    setPosition() {
        let marginLeft;
        let marginTop;
        // 触发的元素
        const targetProps = this.$target[0].getBoundingClientRect();
        // 触发的元素和 Tooltip 之间的距离
        const targetMargin = this.isDesktop() ? 14 : 24;
        // Tooltip 的宽度和高度
        const tooltipWidth = this.$element[0].offsetWidth;
        const tooltipHeight = this.$element[0].offsetHeight;
        // Tooltip 的方向
        let position = this.options.position;
        // 自动判断位置，加 2px，使 Tooltip 距离窗口边框至少有 2px 的间距
        if (position === 'auto') {
            if (targetProps.top +
                targetProps.height +
                targetMargin +
                tooltipHeight +
                2 <
                $window.height()) {
                position = 'bottom';
            }
            else if (targetMargin + tooltipHeight + 2 < targetProps.top) {
                position = 'top';
            }
            else if (targetMargin + tooltipWidth + 2 < targetProps.left) {
                position = 'left';
            }
            else if (targetProps.width + targetMargin + tooltipWidth + 2 <
                $window.width() - targetProps.left) {
                position = 'right';
            }
            else {
                position = 'bottom';
            }
        }
        // 设置位置
        switch (position) {
            case 'bottom':
                marginLeft = -1 * (tooltipWidth / 2);
                marginTop = targetProps.height / 2 + targetMargin;
                this.$element.transformOrigin('top center');
                break;
            case 'top':
                marginLeft = -1 * (tooltipWidth / 2);
                marginTop =
                    -1 * (tooltipHeight + targetProps.height / 2 + targetMargin);
                this.$element.transformOrigin('bottom center');
                break;
            case 'left':
                marginLeft = -1 * (tooltipWidth + targetProps.width / 2 + targetMargin);
                marginTop = -1 * (tooltipHeight / 2);
                this.$element.transformOrigin('center right');
                break;
            case 'right':
                marginLeft = targetProps.width / 2 + targetMargin;
                marginTop = -1 * (tooltipHeight / 2);
                this.$element.transformOrigin('center left');
                break;
        }
        const targetOffset = this.$target.offset();
        this.$element.css({
            top: `${targetOffset.top + targetProps.height / 2}px`,
            left: `${targetOffset.left + targetProps.width / 2}px`,
            'margin-left': `${marginLeft}px`,
            'margin-top': `${marginTop}px`,
        });
    }
    /**
     * 触发组件事件
     * @param name
     */
    triggerEvent(name) {
        componentEvent(name, 'tooltip', this.$target, this);
    }
    /**
     * 动画结束回调
     */
    transitionEnd() {
        if (this.$element.hasClass('mdui-tooltip-open')) {
            this.state = 'opened';
            this.triggerEvent('opened');
        }
        else {
            this.state = 'closed';
            this.triggerEvent('closed');
        }
    }
    /**
     * 当前 tooltip 是否为打开状态
     */
    isOpen() {
        return this.state === 'opening' || this.state === 'opened';
    }
    /**
     * 执行打开 tooltip
     */
    doOpen() {
        this.state = 'opening';
        this.triggerEvent('open');
        this.$element
            .addClass('mdui-tooltip-open')
            .transitionEnd(() => this.transitionEnd());
    }
    /**
     * 打开 Tooltip
     * @param options 允许每次打开时设置不同的参数
     */
    open(options) {
        if (this.isOpen()) {
            return;
        }
        const oldOptions = extend({}, this.options);
        if (options) {
            extend(this.options, options);
        }
        // tooltip 的内容有更新
        if (oldOptions.content !== this.options.content) {
            this.$element.html(this.options.content);
        }
        this.setPosition();
        if (this.options.delay) {
            this.timeoutId = setTimeout(() => this.doOpen(), this.options.delay);
        }
        else {
            this.timeoutId = null;
            this.doOpen();
        }
    }
    /**
     * 关闭 Tooltip
     */
    close() {
        if (this.timeoutId) {
            clearTimeout(this.timeoutId);
            this.timeoutId = null;
        }
        if (!this.isOpen()) {
            return;
        }
        this.state = 'closing';
        this.triggerEvent('close');
        this.$element
            .removeClass('mdui-tooltip-open')
            .transitionEnd(() => this.transitionEnd());
    }
    /**
     * 切换 Tooltip 的打开状态
     */
    toggle() {
        this.isOpen() ? this.close() : this.open();
    }
    /**
     * 获取 Tooltip 状态。共包含四种状态：`opening`、`opened`、`closing`、`closed`
     */
    getState() {
        return this.state;
    }
}
mdui.Tooltip = Tooltip;

const customAttr$4 = 'mdui-tooltip';
const dataName = '_mdui_tooltip';
$(() => {
    // mouseenter 不能冒泡，所以这里用 mouseover 代替
    $document.on('touchstart mouseover', `[${customAttr$4}]`, function () {
        const $target = $(this);
        let instance = $target.data(dataName);
        if (!instance) {
            instance = new mdui.Tooltip(this, parseOptions(this, customAttr$4));
            $target.data(dataName, instance);
        }
    });
});

const container = {};
function queue(name, func) {
    if (isUndefined(container[name])) {
        container[name] = [];
    }
    if (isUndefined(func)) {
        return container[name];
    }
    container[name].push(func);
}
/**
 * 从队列中移除第一个函数，并执行该函数
 * @param name 队列满
 */
function dequeue(name) {
    if (isUndefined(container[name])) {
        return;
    }
    if (!container[name].length) {
        return;
    }
    const func = container[name].shift();
    func();
}

const DEFAULT_OPTIONS$4 = {
    message: '',
    timeout: 4000,
    position: 'bottom',
    buttonText: '',
    buttonColor: '',
    closeOnButtonClick: true,
    closeOnOutsideClick: true,
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    onClick: () => { },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    onButtonClick: () => { },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    onOpen: () => { },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    onOpened: () => { },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    onClose: () => { },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    onClosed: () => { },
};
/**
 * 当前打开着的 Snackbar
 */
let currentInst = null;
/**
 * 队列名
 */
const queueName = '_mdui_snackbar';
class Snackbar {
    constructor(options) {
        /**
         * 配置参数
         */
        this.options = extend({}, DEFAULT_OPTIONS$4);
        /**
         * 当前 Snackbar 的状态
         */
        this.state = 'closed';
        /**
         * setTimeout 的 ID
         */
        this.timeoutId = null;
        extend(this.options, options);
        // 按钮颜色
        let buttonColorStyle = '';
        let buttonColorClass = '';
        if (this.options.buttonColor.indexOf('#') === 0 ||
            this.options.buttonColor.indexOf('rgb') === 0) {
            buttonColorStyle = `style="color:${this.options.buttonColor}"`;
        }
        else if (this.options.buttonColor !== '') {
            buttonColorClass = `mdui-text-color-${this.options.buttonColor}`;
        }
        // 添加 HTML
        this.$element = $('<div class="mdui-snackbar">' +
            `<div class="mdui-snackbar-text">${this.options.message}</div>` +
            (this.options.buttonText
                ? `<a href="javascript:void(0)" class="mdui-snackbar-action mdui-btn mdui-ripple mdui-ripple-white ${buttonColorClass}" ${buttonColorStyle}>${this.options.buttonText}</a>`
                : '') +
            '</div>').appendTo(document.body);
        // 设置位置
        this.setPosition('close');
        this.$element.reflow().addClass(`mdui-snackbar-${this.options.position}`);
    }
    /**
     * 点击 Snackbar 外面的区域关闭
     * @param event
     */
    closeOnOutsideClick(event) {
        const $target = $(event.target);
        if (!$target.hasClass('mdui-snackbar') &&
            !$target.parents('.mdui-snackbar').length) {
            currentInst.close();
        }
    }
    /**
     * 设置 Snackbar 的位置
     * @param state
     */
    setPosition(state) {
        const snackbarHeight = this.$element[0].clientHeight;
        const position = this.options.position;
        let translateX;
        let translateY;
        // translateX
        if (position === 'bottom' || position === 'top') {
            translateX = '-50%';
        }
        else {
            translateX = '0';
        }
        // translateY
        if (state === 'open') {
            translateY = '0';
        }
        else {
            if (position === 'bottom') {
                translateY = snackbarHeight;
            }
            if (position === 'top') {
                translateY = -snackbarHeight;
            }
            if (position === 'left-top' || position === 'right-top') {
                translateY = -snackbarHeight - 24;
            }
            if (position === 'left-bottom' || position === 'right-bottom') {
                translateY = snackbarHeight + 24;
            }
        }
        this.$element.transform(`translate(${translateX},${translateY}px`);
    }
    /**
     * 打开 Snackbar
     */
    open() {
        if (this.state === 'opening' || this.state === 'opened') {
            return;
        }
        // 如果当前有正在显示的 Snackbar，则先加入队列，等旧 Snackbar 关闭后再打开
        if (currentInst) {
            queue(queueName, () => this.open());
            return;
        }
        currentInst = this;
        // 开始打开
        this.state = 'opening';
        this.options.onOpen(this);
        this.setPosition('open');
        this.$element.transitionEnd(() => {
            if (this.state !== 'opening') {
                return;
            }
            this.state = 'opened';
            this.options.onOpened(this);
            // 有按钮时绑定事件
            if (this.options.buttonText) {
                this.$element.find('.mdui-snackbar-action').on('click', () => {
                    this.options.onButtonClick(this);
                    if (this.options.closeOnButtonClick) {
                        this.close();
                    }
                });
            }
            // 点击 snackbar 的事件
            this.$element.on('click', (event) => {
                if (!$(event.target).hasClass('mdui-snackbar-action')) {
                    this.options.onClick(this);
                }
            });
            // 点击 Snackbar 外面的区域关闭
            if (this.options.closeOnOutsideClick) {
                $document.on(startEvent, this.closeOnOutsideClick);
            }
            // 超时后自动关闭
            if (this.options.timeout) {
                this.timeoutId = setTimeout(() => this.close(), this.options.timeout);
            }
        });
    }
    /**
     * 关闭 Snackbar
     */
    close() {
        if (this.state === 'closing' || this.state === 'closed') {
            return;
        }
        if (this.timeoutId) {
            clearTimeout(this.timeoutId);
        }
        if (this.options.closeOnOutsideClick) {
            $document.off(startEvent, this.closeOnOutsideClick);
        }
        this.state = 'closing';
        this.options.onClose(this);
        this.setPosition('close');
        this.$element.transitionEnd(() => {
            if (this.state !== 'closing') {
                return;
            }
            currentInst = null;
            this.state = 'closed';
            this.options.onClosed(this);
            this.$element.remove();
            dequeue(queueName);
        });
    }
}
mdui.snackbar = function (message, options = {}) {
    if (isString(message)) {
        options.message = message;
    }
    else {
        options = message;
    }
    const instance = new Snackbar(options);
    instance.open();
    return instance;
};

$(() => {
    // 切换导航项
    $document.on('click', '.mdui-bottom-nav>a', function () {
        const $item = $(this);
        const $bottomNav = $item.parent();
        $bottomNav.children('a').each((index, item) => {
            const isThis = $item.is(item);
            if (isThis) {
                componentEvent('change', 'bottomNav', $bottomNav[0], undefined, {
                    index,
                });
            }
            isThis
                ? $(item).addClass('mdui-bottom-nav-active')
                : $(item).removeClass('mdui-bottom-nav-active');
        });
    });
    // 滚动时隐藏 mdui-bottom-nav-scroll-hide
    mdui.mutation('.mdui-bottom-nav-scroll-hide', function () {
        new mdui.Headroom(this, {
            pinnedClass: 'mdui-headroom-pinned-down',
            unpinnedClass: 'mdui-headroom-unpinned-down',
        });
    });
});

/**
 * layer 的 HTML 结构
 * @param index
 */
function layerHTML(index = false) {
    return (`<div class="mdui-spinner-layer ${index ? `mdui-spinner-layer-${index}` : ''}">` +
        '<div class="mdui-spinner-circle-clipper mdui-spinner-left">' +
        '<div class="mdui-spinner-circle"></div>' +
        '</div>' +
        '<div class="mdui-spinner-gap-patch">' +
        '<div class="mdui-spinner-circle"></div>' +
        '</div>' +
        '<div class="mdui-spinner-circle-clipper mdui-spinner-right">' +
        '<div class="mdui-spinner-circle"></div>' +
        '</div>' +
        '</div>');
}
/**
 * 填充 HTML
 * @param spinner
 */
function fillHTML(spinner) {
    const $spinner = $(spinner);
    const layer = $spinner.hasClass('mdui-spinner-colorful')
        ? layerHTML(1) + layerHTML(2) + layerHTML(3) + layerHTML(4)
        : layerHTML();
    $spinner.html(layer);
}
$(() => {
    // 页面加载完后自动填充 HTML 结构
    mdui.mutation('.mdui-spinner', function () {
        fillHTML(this);
    });
});
mdui.updateSpinners = function (selector) {
    const $elements = isUndefined(selector) ? $('.mdui-spinner') : $(selector);
    $elements.each(function () {
        fillHTML(this);
    });
};

const DEFAULT_OPTIONS$5 = {
    position: 'auto',
    align: 'auto',
    gutter: 16,
    fixed: false,
    covered: 'auto',
    subMenuTrigger: 'hover',
    subMenuDelay: 200,
};
class Menu {
    constructor(anchorSelector, menuSelector, options = {}) {
        /**
         * 配置参数
         */
        this.options = extend({}, DEFAULT_OPTIONS$5);
        /**
         * 当前菜单状态
         */
        this.state = 'closed';
        this.$anchor = $(anchorSelector).first();
        this.$element = $(menuSelector).first();
        // 触发菜单的元素 和 菜单必须是同级的元素，否则菜单可能不能定位
        if (!this.$anchor.parent().is(this.$element.parent())) {
            throw new Error('anchorSelector and menuSelector must be siblings');
        }
        extend(this.options, options);
        // 是否是级联菜单
        this.isCascade = this.$element.hasClass('mdui-menu-cascade');
        // covered 参数处理
        this.isCovered =
            this.options.covered === 'auto' ? !this.isCascade : this.options.covered;
        // 点击触发菜单切换
        this.$anchor.on('click', () => this.toggle());
        // 点击菜单外面区域关闭菜单
        $document.on('click touchstart', (event) => {
            const $target = $(event.target);
            if (this.isOpen() &&
                !$target.is(this.$element) &&
                !contains(this.$element[0], $target[0]) &&
                !$target.is(this.$anchor) &&
                !contains(this.$anchor[0], $target[0])) {
                this.close();
            }
        });
        // 点击不含子菜单的菜单条目关闭菜单
        // eslint-disable-next-line @typescript-eslint/no-this-alias
        const that = this;
        $document.on('click', '.mdui-menu-item', function () {
            const $item = $(this);
            if (!$item.find('.mdui-menu').length &&
                $item.attr('disabled') === undefined) {
                that.close();
            }
        });
        // 绑定点击或鼠标移入含子菜单的条目的事件
        this.bindSubMenuEvent();
        // 窗口大小变化时，重新调整菜单位置
        $window.on('resize', $.throttle(() => this.readjust(), 100));
    }
    /**
     * 是否为打开状态
     */
    isOpen() {
        return this.state === 'opening' || this.state === 'opened';
    }
    /**
     * 触发组件事件
     * @param name
     */
    triggerEvent(name) {
        componentEvent(name, 'menu', this.$element, this);
    }
    /**
     * 调整主菜单位置
     */
    readjust() {
        let menuLeft;
        let menuTop;
        // 菜单位置和方向
        let position;
        let align;
        // window 窗口的宽度和高度
        const windowHeight = $window.height();
        const windowWidth = $window.width();
        // 配置参数
        const gutter = this.options.gutter;
        const isCovered = this.isCovered;
        const isFixed = this.options.fixed;
        // 动画方向参数
        let transformOriginX;
        let transformOriginY;
        // 菜单的原始宽度和高度
        const menuWidth = this.$element.width();
        const menuHeight = this.$element.height();
        // 触发菜单的元素在窗口中的位置
        const anchorRect = this.$anchor[0].getBoundingClientRect();
        const anchorTop = anchorRect.top;
        const anchorLeft = anchorRect.left;
        const anchorHeight = anchorRect.height;
        const anchorWidth = anchorRect.width;
        const anchorBottom = windowHeight - anchorTop - anchorHeight;
        const anchorRight = windowWidth - anchorLeft - anchorWidth;
        // 触发元素相对其拥有定位属性的父元素的位置
        const anchorOffsetTop = this.$anchor[0].offsetTop;
        const anchorOffsetLeft = this.$anchor[0].offsetLeft;
        // 自动判断菜单位置
        if (this.options.position === 'auto') {
            if (anchorBottom + (isCovered ? anchorHeight : 0) > menuHeight + gutter) {
                // 判断下方是否放得下菜单
                position = 'bottom';
            }
            else if (anchorTop + (isCovered ? anchorHeight : 0) >
                menuHeight + gutter) {
                // 判断上方是否放得下菜单
                position = 'top';
            }
            else {
                // 上下都放不下，居中显示
                position = 'center';
            }
        }
        else {
            position = this.options.position;
        }
        // 自动判断菜单对齐方式
        if (this.options.align === 'auto') {
            if (anchorRight + anchorWidth > menuWidth + gutter) {
                // 判断右侧是否放得下菜单
                align = 'left';
            }
            else if (anchorLeft + anchorWidth > menuWidth + gutter) {
                // 判断左侧是否放得下菜单
                align = 'right';
            }
            else {
                // 左右都放不下，居中显示
                align = 'center';
            }
        }
        else {
            align = this.options.align;
        }
        // 设置菜单位置
        if (position === 'bottom') {
            transformOriginY = '0';
            menuTop =
                (isCovered ? 0 : anchorHeight) +
                    (isFixed ? anchorTop : anchorOffsetTop);
        }
        else if (position === 'top') {
            transformOriginY = '100%';
            menuTop =
                (isCovered ? anchorHeight : 0) +
                    (isFixed ? anchorTop - menuHeight : anchorOffsetTop - menuHeight);
        }
        else {
            transformOriginY = '50%';
            // =====================在窗口中居中
            // 显示的菜单的高度，简单菜单高度不超过窗口高度，若超过了则在菜单内部显示滚动条
            // 级联菜单内部不允许出现滚动条
            let menuHeightTemp = menuHeight;
            // 简单菜单比窗口高时，限制菜单高度
            if (!this.isCascade) {
                if (menuHeight + gutter * 2 > windowHeight) {
                    menuHeightTemp = windowHeight - gutter * 2;
                    this.$element.height(menuHeightTemp);
                }
            }
            menuTop =
                (windowHeight - menuHeightTemp) / 2 +
                    (isFixed ? 0 : anchorOffsetTop - anchorTop);
        }
        this.$element.css('top', `${menuTop}px`);
        // 设置菜单对齐方式
        if (align === 'left') {
            transformOriginX = '0';
            menuLeft = isFixed ? anchorLeft : anchorOffsetLeft;
        }
        else if (align === 'right') {
            transformOriginX = '100%';
            menuLeft = isFixed
                ? anchorLeft + anchorWidth - menuWidth
                : anchorOffsetLeft + anchorWidth - menuWidth;
        }
        else {
            transformOriginX = '50%';
            //=======================在窗口中居中
            // 显示的菜单的宽度，菜单宽度不能超过窗口宽度
            let menuWidthTemp = menuWidth;
            // 菜单比窗口宽，限制菜单宽度
            if (menuWidth + gutter * 2 > windowWidth) {
                menuWidthTemp = windowWidth - gutter * 2;
                this.$element.width(menuWidthTemp);
            }
            menuLeft =
                (windowWidth - menuWidthTemp) / 2 +
                    (isFixed ? 0 : anchorOffsetLeft - anchorLeft);
        }
        this.$element.css('left', `${menuLeft}px`);
        // 设置菜单动画方向
        this.$element.transformOrigin(`${transformOriginX} ${transformOriginY}`);
    }
    /**
     * 调整子菜单的位置
     * @param $submenu
     */
    readjustSubmenu($submenu) {
        const $item = $submenu.parent('.mdui-menu-item');
        let submenuTop;
        let submenuLeft;
        // 子菜单位置和方向
        let position;
        let align;
        // window 窗口的宽度和高度
        const windowHeight = $window.height();
        const windowWidth = $window.width();
        // 动画方向参数
        let transformOriginX;
        let transformOriginY;
        // 子菜单的原始宽度和高度
        const submenuWidth = $submenu.width();
        const submenuHeight = $submenu.height();
        // 触发子菜单的菜单项的宽度高度
        const itemRect = $item[0].getBoundingClientRect();
        const itemWidth = itemRect.width;
        const itemHeight = itemRect.height;
        const itemLeft = itemRect.left;
        const itemTop = itemRect.top;
        // 判断菜单上下位置
        if (windowHeight - itemTop > submenuHeight) {
            // 判断下方是否放得下菜单
            position = 'bottom';
        }
        else if (itemTop + itemHeight > submenuHeight) {
            // 判断上方是否放得下菜单
            position = 'top';
        }
        else {
            // 默认放在下方
            position = 'bottom';
        }
        // 判断菜单左右位置
        if (windowWidth - itemLeft - itemWidth > submenuWidth) {
            // 判断右侧是否放得下菜单
            align = 'left';
        }
        else if (itemLeft > submenuWidth) {
            // 判断左侧是否放得下菜单
            align = 'right';
        }
        else {
            // 默认放在右侧
            align = 'left';
        }
        // 设置菜单位置
        if (position === 'bottom') {
            transformOriginY = '0';
            submenuTop = '0';
        }
        else if (position === 'top') {
            transformOriginY = '100%';
            submenuTop = -submenuHeight + itemHeight;
        }
        $submenu.css('top', `${submenuTop}px`);
        // 设置菜单对齐方式
        if (align === 'left') {
            transformOriginX = '0';
            submenuLeft = itemWidth;
        }
        else if (align === 'right') {
            transformOriginX = '100%';
            submenuLeft = -submenuWidth;
        }
        $submenu.css('left', `${submenuLeft}px`);
        // 设置菜单动画方向
        $submenu.transformOrigin(`${transformOriginX} ${transformOriginY}`);
    }
    /**
     * 打开子菜单
     * @param $submenu
     */
    openSubMenu($submenu) {
        this.readjustSubmenu($submenu);
        $submenu
            .addClass('mdui-menu-open')
            .parent('.mdui-menu-item')
            .addClass('mdui-menu-item-active');
    }
    /**
     * 关闭子菜单，及其嵌套的子菜单
     * @param $submenu
     */
    closeSubMenu($submenu) {
        // 关闭子菜单
        $submenu
            .removeClass('mdui-menu-open')
            .addClass('mdui-menu-closing')
            .transitionEnd(() => $submenu.removeClass('mdui-menu-closing'))
            // 移除激活状态的样式
            .parent('.mdui-menu-item')
            .removeClass('mdui-menu-item-active');
        // 循环关闭嵌套的子菜单
        $submenu.find('.mdui-menu').each((_, menu) => {
            const $subSubmenu = $(menu);
            $subSubmenu
                .removeClass('mdui-menu-open')
                .addClass('mdui-menu-closing')
                .transitionEnd(() => $subSubmenu.removeClass('mdui-menu-closing'))
                .parent('.mdui-menu-item')
                .removeClass('mdui-menu-item-active');
        });
    }
    /**
     * 切换子菜单状态
     * @param $submenu
     */
    toggleSubMenu($submenu) {
        $submenu.hasClass('mdui-menu-open')
            ? this.closeSubMenu($submenu)
            : this.openSubMenu($submenu);
    }
    /**
     * 绑定子菜单事件
     */
    bindSubMenuEvent() {
        // eslint-disable-next-line @typescript-eslint/no-this-alias
        const that = this;
        // 点击打开子菜单
        this.$element.on('click', '.mdui-menu-item', function (event) {
            const $item = $(this);
            const $target = $(event.target);
            // 禁用状态菜单不操作
            if ($item.attr('disabled') !== undefined) {
                return;
            }
            // 没有点击在子菜单的菜单项上时，不操作（点在了子菜单的空白区域、或分隔线上）
            if ($target.is('.mdui-menu') || $target.is('.mdui-divider')) {
                return;
            }
            // 阻止冒泡，点击菜单项时只在最后一级的 mdui-menu-item 上生效，不向上冒泡
            if (!$target.parents('.mdui-menu-item').first().is($item)) {
                return;
            }
            // 当前菜单的子菜单
            const $submenu = $item.children('.mdui-menu');
            // 先关闭除当前子菜单外的所有同级子菜单
            $item
                .parent('.mdui-menu')
                .children('.mdui-menu-item')
                .each((_, item) => {
                const $tmpSubmenu = $(item).children('.mdui-menu');
                if ($tmpSubmenu.length &&
                    (!$submenu.length || !$tmpSubmenu.is($submenu))) {
                    that.closeSubMenu($tmpSubmenu);
                }
            });
            // 切换当前子菜单
            if ($submenu.length) {
                that.toggleSubMenu($submenu);
            }
        });
        if (this.options.subMenuTrigger === 'hover') {
            // 临时存储 setTimeout 对象
            let timeout = null;
            let timeoutOpen = null;
            this.$element.on('mouseover mouseout', '.mdui-menu-item', function (event) {
                const $item = $(this);
                const eventType = event.type;
                const $relatedTarget = $(event.relatedTarget);
                // 禁用状态的菜单不操作
                if ($item.attr('disabled') !== undefined) {
                    return;
                }
                // 用 mouseover 模拟 mouseenter
                if (eventType === 'mouseover') {
                    if (!$item.is($relatedTarget) &&
                        contains($item[0], $relatedTarget[0])) {
                        return;
                    }
                }
                // 用 mouseout 模拟 mouseleave
                else if (eventType === 'mouseout') {
                    if ($item.is($relatedTarget) ||
                        contains($item[0], $relatedTarget[0])) {
                        return;
                    }
                }
                // 当前菜单项下的子菜单，未必存在
                const $submenu = $item.children('.mdui-menu');
                // 鼠标移入菜单项时，显示菜单项下的子菜单
                if (eventType === 'mouseover') {
                    if ($submenu.length) {
                        // 当前子菜单准备打开时，如果当前子菜单正准备着关闭，不用再关闭了
                        const tmpClose = $submenu.data('timeoutClose.mdui.menu');
                        if (tmpClose) {
                            clearTimeout(tmpClose);
                        }
                        // 如果当前子菜单已经打开，不操作
                        if ($submenu.hasClass('mdui-menu-open')) {
                            return;
                        }
                        // 当前子菜单准备打开时，其他准备打开的子菜单不用再打开了
                        clearTimeout(timeoutOpen);
                        // 准备打开当前子菜单
                        timeout = timeoutOpen = setTimeout(() => that.openSubMenu($submenu), that.options.subMenuDelay);
                        $submenu.data('timeoutOpen.mdui.menu', timeout);
                    }
                }
                // 鼠标移出菜单项时，关闭菜单项下的子菜单
                else if (eventType === 'mouseout') {
                    if ($submenu.length) {
                        // 鼠标移出菜单项时，如果当前菜单项下的子菜单正准备打开，不用再打开了
                        const tmpOpen = $submenu.data('timeoutOpen.mdui.menu');
                        if (tmpOpen) {
                            clearTimeout(tmpOpen);
                        }
                        // 准备关闭当前子菜单
                        timeout = setTimeout(() => that.closeSubMenu($submenu), that.options.subMenuDelay);
                        $submenu.data('timeoutClose.mdui.menu', timeout);
                    }
                }
            });
        }
    }
    /**
     * 动画结束回调
     */
    transitionEnd() {
        this.$element.removeClass('mdui-menu-closing');
        if (this.state === 'opening') {
            this.state = 'opened';
            this.triggerEvent('opened');
        }
        if (this.state === 'closing') {
            this.state = 'closed';
            this.triggerEvent('closed');
            // 关闭后，恢复菜单样式到默认状态，并恢复 fixed 定位
            this.$element.css({
                top: '',
                left: '',
                width: '',
                position: 'fixed',
            });
        }
    }
    /**
     * 切换菜单状态
     */
    toggle() {
        this.isOpen() ? this.close() : this.open();
    }
    /**
     * 打开菜单
     */
    open() {
        if (this.isOpen()) {
            return;
        }
        this.state = 'opening';
        this.triggerEvent('open');
        this.readjust();
        this.$element
            // 菜单隐藏状态使用使用 fixed 定位。
            .css('position', this.options.fixed ? 'fixed' : 'absolute')
            .addClass('mdui-menu-open')
            .transitionEnd(() => this.transitionEnd());
    }
    /**
     * 关闭菜单
     */
    close() {
        if (!this.isOpen()) {
            return;
        }
        this.state = 'closing';
        this.triggerEvent('close');
        // 菜单开始关闭时，关闭所有子菜单
        this.$element.find('.mdui-menu').each((_, submenu) => {
            this.closeSubMenu($(submenu));
        });
        this.$element
            .removeClass('mdui-menu-open')
            .addClass('mdui-menu-closing')
            .transitionEnd(() => this.transitionEnd());
    }
}
mdui.Menu = Menu;

const customAttr$5 = 'mdui-menu';
const dataName$1 = '_mdui_menu';
$(() => {
    $document.on('click', `[${customAttr$5}]`, function () {
        const $this = $(this);
        let instance = $this.data(dataName$1);
        if (!instance) {
            const options = parseOptions(this, customAttr$5);
            const menuSelector = options.target;
            // @ts-ignore
            delete options.target;
            instance = new mdui.Menu($this, menuSelector, options);
            $this.data(dataName$1, instance);
            instance.toggle();
        }
    });
});

export default mdui;
//# sourceMappingURL=mdui.esm.js.map