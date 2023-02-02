/*!
 * mdui 1.0.2 (https://mdui.org)
 * Copyright 2016-2023 zdhxiong
 * Licensed under MIT
 */
(function (global, factory) {
  typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
  typeof define === 'function' && define.amd ? define(factory) :
  (global = typeof globalThis !== 'undefined' ? globalThis : global || self, global.mdui = factory());
}(this, (function () { 'use strict';

  !function(){try{return new MouseEvent("test")}catch(e$1){}var e=function(e,t){t=t||{bubbles:!1,cancelable:!1};var n=document.createEvent("MouseEvent");return n.initMouseEvent(e,t.bubbles,t.cancelable,window,0,t.screenX||0,t.screenY||0,t.clientX||0,t.clientY||0,t.ctrlKey||!1,t.altKey||!1,t.shiftKey||!1,t.metaKey||!1,t.button||0,t.relatedTarget||null),n};e.prototype=Event.prototype,window.MouseEvent=e;}();

  !function(){function t(t,e){e=e||{bubbles:!1,cancelable:!1,detail:void 0};var n=document.createEvent("CustomEvent");return n.initCustomEvent(t,e.bubbles,e.cancelable,e.detail),n}"function"!=typeof window.CustomEvent&&(t.prototype=window.Event.prototype,window.CustomEvent=t);}();

  /**
   * @this {Promise}
   */
  function finallyConstructor(callback) {
    var constructor = this.constructor;
    return this.then(
      function(value) {
        // @ts-ignore
        return constructor.resolve(callback()).then(function() {
          return value;
        });
      },
      function(reason) {
        // @ts-ignore
        return constructor.resolve(callback()).then(function() {
          // @ts-ignore
          return constructor.reject(reason);
        });
      }
    );
  }

  function allSettled(arr) {
    var P = this;
    return new P(function(resolve, reject) {
      if (!(arr && typeof arr.length !== 'undefined')) {
        return reject(
          new TypeError(
            typeof arr +
              ' ' +
              arr +
              ' is not iterable(cannot read property Symbol(Symbol.iterator))'
          )
        );
      }
      var args = Array.prototype.slice.call(arr);
      if (args.length === 0) { return resolve([]); }
      var remaining = args.length;

      function res(i, val) {
        if (val && (typeof val === 'object' || typeof val === 'function')) {
          var then = val.then;
          if (typeof then === 'function') {
            then.call(
              val,
              function(val) {
                res(i, val);
              },
              function(e) {
                args[i] = { status: 'rejected', reason: e };
                if (--remaining === 0) {
                  resolve(args);
                }
              }
            );
            return;
          }
        }
        args[i] = { status: 'fulfilled', value: val };
        if (--remaining === 0) {
          resolve(args);
        }
      }

      for (var i = 0; i < args.length; i++) {
        res(i, args[i]);
      }
    });
  }

  // Store setTimeout reference so promise-polyfill will be unaffected by
  // other code modifying setTimeout (like sinon.useFakeTimers())
  var setTimeoutFunc = setTimeout;

  function isArray(x) {
    return Boolean(x && typeof x.length !== 'undefined');
  }

  function noop() {}

  // Polyfill for Function.prototype.bind
  function bind(fn, thisArg) {
    return function() {
      fn.apply(thisArg, arguments);
    };
  }

  /**
   * @constructor
   * @param {Function} fn
   */
  function Promise$1(fn) {
    if (!(this instanceof Promise$1))
      { throw new TypeError('Promises must be constructed via new'); }
    if (typeof fn !== 'function') { throw new TypeError('not a function'); }
    /** @type {!number} */
    this._state = 0;
    /** @type {!boolean} */
    this._handled = false;
    /** @type {Promise|undefined} */
    this._value = undefined;
    /** @type {!Array<!Function>} */
    this._deferreds = [];

    doResolve(fn, this);
  }

  function handle(self, deferred) {
    while (self._state === 3) {
      self = self._value;
    }
    if (self._state === 0) {
      self._deferreds.push(deferred);
      return;
    }
    self._handled = true;
    Promise$1._immediateFn(function() {
      var cb = self._state === 1 ? deferred.onFulfilled : deferred.onRejected;
      if (cb === null) {
        (self._state === 1 ? resolve : reject)(deferred.promise, self._value);
        return;
      }
      var ret;
      try {
        ret = cb(self._value);
      } catch (e) {
        reject(deferred.promise, e);
        return;
      }
      resolve(deferred.promise, ret);
    });
  }

  function resolve(self, newValue) {
    try {
      // Promise Resolution Procedure: https://github.com/promises-aplus/promises-spec#the-promise-resolution-procedure
      if (newValue === self)
        { throw new TypeError('A promise cannot be resolved with itself.'); }
      if (
        newValue &&
        (typeof newValue === 'object' || typeof newValue === 'function')
      ) {
        var then = newValue.then;
        if (newValue instanceof Promise$1) {
          self._state = 3;
          self._value = newValue;
          finale(self);
          return;
        } else if (typeof then === 'function') {
          doResolve(bind(then, newValue), self);
          return;
        }
      }
      self._state = 1;
      self._value = newValue;
      finale(self);
    } catch (e) {
      reject(self, e);
    }
  }

  function reject(self, newValue) {
    self._state = 2;
    self._value = newValue;
    finale(self);
  }

  function finale(self) {
    if (self._state === 2 && self._deferreds.length === 0) {
      Promise$1._immediateFn(function() {
        if (!self._handled) {
          Promise$1._unhandledRejectionFn(self._value);
        }
      });
    }

    for (var i = 0, len = self._deferreds.length; i < len; i++) {
      handle(self, self._deferreds[i]);
    }
    self._deferreds = null;
  }

  /**
   * @constructor
   */
  function Handler(onFulfilled, onRejected, promise) {
    this.onFulfilled = typeof onFulfilled === 'function' ? onFulfilled : null;
    this.onRejected = typeof onRejected === 'function' ? onRejected : null;
    this.promise = promise;
  }

  /**
   * Take a potentially misbehaving resolver function and make sure
   * onFulfilled and onRejected are only called once.
   *
   * Makes no guarantees about asynchrony.
   */
  function doResolve(fn, self) {
    var done = false;
    try {
      fn(
        function(value) {
          if (done) { return; }
          done = true;
          resolve(self, value);
        },
        function(reason) {
          if (done) { return; }
          done = true;
          reject(self, reason);
        }
      );
    } catch (ex) {
      if (done) { return; }
      done = true;
      reject(self, ex);
    }
  }

  Promise$1.prototype['catch'] = function(onRejected) {
    return this.then(null, onRejected);
  };

  Promise$1.prototype.then = function(onFulfilled, onRejected) {
    // @ts-ignore
    var prom = new this.constructor(noop);

    handle(this, new Handler(onFulfilled, onRejected, prom));
    return prom;
  };

  Promise$1.prototype['finally'] = finallyConstructor;

  Promise$1.all = function(arr) {
    return new Promise$1(function(resolve, reject) {
      if (!isArray(arr)) {
        return reject(new TypeError('Promise.all accepts an array'));
      }

      var args = Array.prototype.slice.call(arr);
      if (args.length === 0) { return resolve([]); }
      var remaining = args.length;

      function res(i, val) {
        try {
          if (val && (typeof val === 'object' || typeof val === 'function')) {
            var then = val.then;
            if (typeof then === 'function') {
              then.call(
                val,
                function(val) {
                  res(i, val);
                },
                reject
              );
              return;
            }
          }
          args[i] = val;
          if (--remaining === 0) {
            resolve(args);
          }
        } catch (ex) {
          reject(ex);
        }
      }

      for (var i = 0; i < args.length; i++) {
        res(i, args[i]);
      }
    });
  };

  Promise$1.allSettled = allSettled;

  Promise$1.resolve = function(value) {
    if (value && typeof value === 'object' && value.constructor === Promise$1) {
      return value;
    }

    return new Promise$1(function(resolve) {
      resolve(value);
    });
  };

  Promise$1.reject = function(value) {
    return new Promise$1(function(resolve, reject) {
      reject(value);
    });
  };

  Promise$1.race = function(arr) {
    return new Promise$1(function(resolve, reject) {
      if (!isArray(arr)) {
        return reject(new TypeError('Promise.race accepts an array'));
      }

      for (var i = 0, len = arr.length; i < len; i++) {
        Promise$1.resolve(arr[i]).then(resolve, reject);
      }
    });
  };

  // Use polyfill for setImmediate for performance gains
  Promise$1._immediateFn =
    // @ts-ignore
    (typeof setImmediate === 'function' &&
      function(fn) {
        // @ts-ignore
        setImmediate(fn);
      }) ||
    function(fn) {
      setTimeoutFunc(fn, 0);
    };

  Promise$1._unhandledRejectionFn = function _unhandledRejectionFn(err) {
    if (typeof console !== 'undefined' && console) {
      console.warn('Possible Unhandled Promise Rejection:', err); // eslint-disable-line no-console
    }
  };

  /** @suppress {undefinedVars} */
  var globalNS = (function() {
    // the only reliable means to get the global object is
    // `Function('return this')()`
    // However, this causes CSP violations in Chrome apps.
    if (typeof self !== 'undefined') {
      return self;
    }
    if (typeof window !== 'undefined') {
      return window;
    }
    if (typeof global !== 'undefined') {
      return global;
    }
    throw new Error('unable to locate global object');
  })();

  // Expose the polyfill if Promise is undefined or set to a
  // non-function value. The latter can be due to a named HTMLElement
  // being exposed by browsers for legacy reasons.
  // https://github.com/taylorhakes/promise-polyfill/issues/114
  if (typeof globalNS['Promise'] !== 'function') {
    globalNS['Promise'] = Promise$1;
  } else if (!globalNS.Promise.prototype['finally']) {
    globalNS.Promise.prototype['finally'] = finallyConstructor;
  } else if (!globalNS.Promise.allSettled) {
    globalNS.Promise.allSettled = allSettled;
  }

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
          .replace(/-([a-z])/g, function (_, letter) { return letter.toUpperCase(); });
  }
  /**
   * 把驼峰法转为用 - 分隔的字符串（如 boxSizing 转换为 box-sizing）
   * @param string
   */
  function toKebabCase(string) {
      return string.replace(/[A-Z]/g, function (replacer) { return '-' + replacer.toLowerCase(); });
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
      var position = direction === 'width' ? ['Left', 'Right'] : ['Top', 'Bottom'];
      return [0, 1].reduce(function (prev, _, index) {
          var prop = extra + position[index];
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
          var valueNumber = element.getBoundingClientRect()[name];
          if (isBorderBox(element)) {
              return (valueNumber + "px");
          }
          return ((valueNumber -
              getExtraWidth(element, name, 'border') -
              getExtraWidth(element, name, 'padding')) + "px");
      }
      return getComputedStyleValue(element, name);
  }
  /**
   * 获取子节点组成的数组
   * @param target
   * @param parent
   */
  function getChildNodesArray(target, parent) {
      var tempParent = document.createElement(parent);
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
  var cssNumber = [
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
      'zoom' ];

  function each(target, callback) {
      if (isArrayLike(target)) {
          for (var i = 0; i < target.length; i += 1) {
              if (callback.call(target[i], i, target[i]) === false) {
                  return target;
              }
          }
      }
      else {
          var keys = Object.keys(target);
          for (var i$1 = 0; i$1 < keys.length; i$1 += 1) {
              if (callback.call(target[keys[i$1]], keys[i$1], target[keys[i$1]]) === false) {
                  return target;
              }
          }
      }
      return target;
  }

  /**
   * 为了使用模块扩充，这里不能使用默认导出
   */
  var JQ = function JQ(arr) {
      var this$1 = this;

      this.length = 0;
      if (!arr) {
          return this;
      }
      each(arr, function (i, item) {
          // @ts-ignore
          this$1[i] = item;
      });
      this.length = arr.length;
      return this;
  };

  function get$() {
      var $ = function (selector) {
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
                  document.addEventListener('DOMContentLoaded', function () { return selector.call(document, $); }, false);
              }
              return new JQ([document]);
          }
          // String
          if (isString(selector)) {
              var html = selector.trim();
              // 根据 HTML 字符串创建 JQ 对象
              if (html[0] === '<' && html[html.length - 1] === '>') {
                  var toCreate = 'div';
                  var tags = {
                      li: 'ul',
                      tr: 'tbody',
                      td: 'tr',
                      th: 'tr',
                      tbody: 'table',
                      option: 'select',
                  };
                  each(tags, function (childTag, parentTag) {
                      if (html.indexOf(("<" + childTag)) === 0) {
                          toCreate = parentTag;
                          return false;
                      }
                      return;
                  });
                  return new JQ(getChildNodesArray(html, toCreate));
              }
              // 根据 CSS 选择器创建 JQ 对象
              var isIdSelector = selector[0] === '#' && !selector.match(/[ .<>:~]/);
              if (!isIdSelector) {
                  return new JQ(document.querySelectorAll(selector));
              }
              var element = document.getElementById(selector.slice(1));
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
  var $ = get$();

  var mdui = {
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
      each(second, function (_, value) {
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
      var foundElements = [];
      this.each(function (_, element) {
          merge(foundElements, $(element.querySelectorAll(selector)).get());
      });
      return new JQ(foundElements);
  };

  // 存储事件
  var handlers = {};
  // 元素ID
  var mduiElementId = 1;
  /**
   * 为元素赋予一个唯一的ID
   */
  function getElementId(element) {
      var key = '_mduiEventId';
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
      var parts = type.split('.');
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
      var event = parse(type);
      return (handlers[getElementId(element)] || []).filter(function (handler) { return handler &&
          (!event.type || handler.type === event.type) &&
          (!event.ns || matcherFor(event.ns).test(handler.ns)) &&
          (!func || getElementId(handler.func) === getElementId(func)) &&
          (!selector || handler.selector === selector); });
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
      var elementId = getElementId(element);
      if (!handlers[elementId]) {
          handlers[elementId] = [];
      }
      // 传入 data.useCapture 来设置 useCapture: true
      var useCapture = false;
      if (isObjectLike(data) && data.useCapture) {
          useCapture = true;
      }
      types.split(' ').forEach(function (type) {
          if (!type) {
              return;
          }
          var event = parse(type);
          function callFn(e, elem) {
              // 因为鼠标事件模拟事件的 detail 属性是只读的，因此在 e._detail 中存储参数
              var result = func.apply(elem, 
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
                      .forEach(function (elem) {
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
          var handler = {
              type: event.type,
              ns: event.ns,
              func: func,
              selector: selector,
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
      var handlersInElement = handlers[getElementId(element)] || [];
      var removeEvent = function (handler) {
          delete handlersInElement[handler.id];
          element.removeEventListener(handler.type, handler.proxy, false);
      };
      if (!types) {
          handlersInElement.forEach(function (handler) { return removeEvent(handler); });
      }
      else {
          types.split(' ').forEach(function (type) {
              if (type) {
                  getHandlers(element, type, func, selector).forEach(function (handler) { return removeEvent(handler); });
              }
          });
      }
  }

  $.fn.trigger = function (type, extraParameters) {
      var event = parse(type);
      var eventObject;
      var eventParams = {
          bubbles: true,
          cancelable: true,
      };
      var isMouseEvent = ['click', 'mousedown', 'mouseup', 'mousemove'].indexOf(event.type) > -1;
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

  function extend(target, object1) {
      var objectN = [], len = arguments.length - 2;
      while ( len-- > 0 ) objectN[ len ] = arguments[ len + 2 ];

      objectN.unshift(object1);
      each(objectN, function (_, object) {
          each(object, function (prop, value) {
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
      var args = [];
      function destructure(key, value) {
          var keyTmp;
          if (isObjectLike(value)) {
              each(value, function (i, v) {
                  if (Array.isArray(value) && !isObjectLike(v)) {
                      keyTmp = '';
                  }
                  else {
                      keyTmp = i;
                  }
                  destructure((key + "[" + keyTmp + "]"), v);
              });
          }
          else {
              if (value == null || value === '') {
                  keyTmp = '=';
              }
              else {
                  keyTmp = "=" + (encodeURIComponent(value));
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
  var globalOptions = {};
  // 全局事件名
  var ajaxEvents = {
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
      return (url + "&" + query).replace(/[&?]{1,2}/, '?');
  }
  /**
   * 合并请求参数，参数优先级：options > globalOptions > defaults
   * @param options
   */
  function mergeOptions(options) {
      // 默认参数
      var defaults = {
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
      each(globalOptions, function (key, value) {
          var callbacks = [
              'beforeSend',
              'success',
              'error',
              'complete',
              'statusCode' ];
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
      var isCanceled = false;
      // 事件参数
      var eventParams = {};
      // 参数合并
      var mergedOptions = mergeOptions(options);
      var url = mergedOptions.url || window.location.toString();
      var method = mergedOptions.method.toUpperCase();
      var data = mergedOptions.data;
      var processData = mergedOptions.processData;
      var async = mergedOptions.async;
      var cache = mergedOptions.cache;
      var username = mergedOptions.username;
      var password = mergedOptions.password;
      var headers = mergedOptions.headers;
      var xhrFields = mergedOptions.xhrFields;
      var statusCode = mergedOptions.statusCode;
      var dataType = mergedOptions.dataType;
      var contentType = mergedOptions.contentType;
      var timeout = mergedOptions.timeout;
      var global = mergedOptions.global;
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
      function trigger(event, params, callback) {
          var args = [], len = arguments.length - 3;
          while ( len-- > 0 ) args[ len ] = arguments[ len + 3 ];

          // 触发全局事件
          if (global) {
              $(document).trigger(event, params);
          }
          // 触发 ajax 回调和事件
          var result1;
          var result2;
          if (callback) {
              // 全局回调
              if (callback in globalOptions) {
                  // @ts-ignore
                  result1 = globalOptions[callback].apply(globalOptions, args);
              }
              // 自定义回调
              if (mergedOptions[callback]) {
                  // @ts-ignore
                  result2 = mergedOptions[callback].apply(mergedOptions, args);
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
          var textStatus;
          return new Promise(function (resolve, reject) {
              // GET/HEAD 请求的缓存处理
              if (isQueryStringData(method) && !cache) {
                  url = appendQuery(url, ("_=" + (Date.now())));
              }
              // 创建 XHR
              var xhr = new XMLHttpRequest();
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
                  each(headers, function (key, value) {
                      // undefined 值不发送，string 和 null 需要发送
                      if (!isUndefined(value)) {
                          xhr.setRequestHeader(key, value + ''); // 把 null 转换成字符串
                      }
                  });
              }
              // 检查是否是跨域请求，跨域请求时不添加 X-Requested-With
              var crossDomain = /^([\w-]+:)?\/\/([^/]+)/.test(url) &&
                  RegExp.$2 !== window.location.host;
              if (!crossDomain) {
                  xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
              }
              if (xhrFields) {
                  each(xhrFields, function (key, value) {
                      // @ts-ignore
                      xhr[key] = value;
                  });
              }
              eventParams.xhr = xhr;
              eventParams.options = mergedOptions;
              var xhrTimeout;
              xhr.onload = function () {
                  if (xhrTimeout) {
                      clearTimeout(xhrTimeout);
                  }
                  // AJAX 返回的 HTTP 响应码是否表示成功
                  var isHttpStatusSuccess = (xhr.status >= 200 && xhr.status < 300) ||
                      xhr.status === 304 ||
                      xhr.status === 0;
                  var responseData;
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
                  each([globalOptions.statusCode, statusCode], function (_, func) {
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
                  var statusText = 'abort';
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
                  xhrTimeout = setTimeout(function () {
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

  var dataNS = '_mduiElementDataStorage';

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
      each(object, function (key, value) {
          // @ts-ignore
          element[dataNS][toCamelCase(key)] = value;
      });
  }
  function data(element, key, value) {
      var obj;

      // 根据键值对设置值
      // data(element, { 'key' : 'value' })
      if (isObjectLike(key)) {
          setObjectToElement(element, key);
          return key;
      }
      // 根据 key、value 设置值
      // data(element, 'key', 'value')
      if (!isUndefined(value)) {
          setObjectToElement(element, ( obj = {}, obj[key] = value, obj ));
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

  $.extend = function () {
      var this$1 = this;
      var objectN = [], len = arguments.length;
      while ( len-- ) objectN[ len ] = arguments[ len ];

      if (objectN.length === 1) {
          each(objectN[0], function (prop, value) {
              this$1[prop] = value;
          });
          return this;
      }
      return extend.apply(void 0, [ objectN.shift(), objectN.shift() ].concat( objectN ));
  };

  function map(elements, callback) {
      var ref;

      var value;
      var ret = [];
      each(elements, function (i, element) {
          value = callback.call(window, element, i);
          if (value != null) {
              ret.push(value);
          }
      });
      return (ref = []).concat.apply(ref, ret);
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
      var remove = function (nameItem) {
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
              .filter(function (nameItem) { return nameItem; })
              .forEach(function (nameItem) { return remove(nameItem); });
      }
      else {
          each(name, function (_, nameItem) { return remove(nameItem); });
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
      var result = [];
      each(arr, function (_, val) {
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

  each(['add', 'remove', 'toggle'], function (_, name) {
      $.fn[(name + "Class")] = function (className) {
          if (name === 'remove' && !arguments.length) {
              return this.each(function (_, element) {
                  element.setAttribute('class', '');
              });
          }
          return this.each(function (i, element) {
              if (!isElement(element)) {
                  return;
              }
              var classes = (isFunction(className)
                  ? className.call(element, i, element.getAttribute('class') || '')
                  : className)
                  .split(' ')
                  .filter(function (name) { return name; });
              each(classes, function (_, cls) {
                  element.classList[name](cls);
              });
          });
      };
  });

  each(['insertBefore', 'insertAfter'], function (nameIndex, name) {
      $.fn[name] = function (target) {
          var $element = nameIndex ? $(this.get().reverse()) : this; // 顺序和 jQuery 保持一致
          var $target = $(target);
          var result = [];
          $target.each(function (index, target) {
              if (!target.parentNode) {
                  return;
              }
              $element.each(function (_, element) {
                  var newItem = index
                      ? element.cloneNode(true)
                      : element;
                  var existingItem = nameIndex ? target.nextSibling : target;
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
  each(['before', 'after'], function (nameIndex, name) {
      $.fn[name] = function () {
          var args = [], len = arguments.length;
          while ( len-- ) args[ len ] = arguments[ len ];

          // after 方法，多个参数需要按参数顺序添加到元素后面，所以需要将参数顺序反向处理
          if (nameIndex === 1) {
              args = args.reverse();
          }
          return this.each(function (index, element) {
              var targets = isFunction(args[0])
                  ? [args[0].call(element, index, element.innerHTML)]
                  : args;
              each(targets, function (_, target) {
                  var $target;
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
      var this$1 = this;

      // types 是对象
      if (isObjectLike(types)) {
          each(types, function (type, fn) {
              // this.off('click', undefined, function () {})
              // this.off('click', '.box', function () {})
              this$1.off(type, selector, fn);
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
      var this$1 = this;

      // types 可以是 type/func 对象
      if (isObjectLike(types)) {
          // (types-Object, selector, data)
          if (!isString(selector)) {
              // (types-Object, data)
              data = data || selector;
              selector = undefined;
          }
          each(types, function (type, fn) {
              // selector 和 data 都可能是 undefined
              // @ts-ignore
              this$1.on(type, selector, data, fn, one);
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
          var _this = this;
          var origCallback = callback;
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

  each(ajaxEvents, function (name, eventName) {
      $.fn[name] = function (fn) {
          return this.on(eventName, function (e, params) {
              fn(e, params.xhr, params.options, params.data);
          });
      };
  });

  $.fn.map = function (callback) {
      return new JQ(map(this, function (element, i) { return callback.call(element, i, element); }));
  };

  $.fn.clone = function () {
      return this.map(function () {
          return this.cloneNode(true);
      });
  };

  $.fn.is = function (selector) {
      var isMatched = false;
      if (isFunction(selector)) {
          this.each(function (index, element) {
              if (selector.call(element, index, element)) {
                  isMatched = true;
              }
          });
          return isMatched;
      }
      if (isString(selector)) {
          this.each(function (_, element) {
              if (isDocument(element) || isWindow(element)) {
                  return;
              }
              // @ts-ignore
              var matches = element.matches || element.msMatchesSelector;
              if (matches.call(element, selector)) {
                  isMatched = true;
              }
          });
          return isMatched;
      }
      var $compareWith = $(selector);
      this.each(function (_, element) {
          $compareWith.each(function (_, compare) {
              if (element === compare) {
                  isMatched = true;
              }
          });
      });
      return isMatched;
  };

  $.fn.remove = function (selector) {
      return this.each(function (_, element) {
          if (element.parentNode && (!selector || $(element).is(selector))) {
              element.parentNode.removeChild(element);
          }
      });
  };

  each(['prepend', 'append'], function (nameIndex, name) {
      $.fn[name] = function () {
          var args = [], len = arguments.length;
          while ( len-- ) args[ len ] = arguments[ len ];

          return this.each(function (index, element) {
              var ref;

              var childNodes = element.childNodes;
              var childLength = childNodes.length;
              var child = childLength
                  ? childNodes[nameIndex ? childLength - 1 : 0]
                  : document.createElement('div');
              if (!childLength) {
                  element.appendChild(child);
              }
              var contents = isFunction(args[0])
                  ? [args[0].call(element, index, element.innerHTML)]
                  : args;
              // 如果不是字符串，则仅第一个元素使用原始元素，其他的都克隆自第一个元素
              if (index) {
                  contents = contents.map(function (content) {
                      return isString(content) ? content : $(content).clone();
                  });
              }
              (ref = $(child))[nameIndex ? 'after' : 'before'].apply(ref, contents);
              if (!childLength) {
                  element.removeChild(child);
              }
          });
      };
  });

  each(['appendTo', 'prependTo'], function (nameIndex, name) {
      $.fn[name] = function (target) {
          var extraChilds = [];
          var $target = $(target).map(function (_, element) {
              var childNodes = element.childNodes;
              var childLength = childNodes.length;
              if (childLength) {
                  return childNodes[nameIndex ? 0 : childLength - 1];
              }
              var child = document.createElement('div');
              element.appendChild(child);
              extraChilds.push(child);
              return child;
          });
          var $result = this[nameIndex ? 'insertBefore' : 'insertAfter']($target);
          $(extraChilds).remove();
          return $result;
      };
  });

  each(['attr', 'prop', 'css'], function (nameIndex, name) {
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
                      ? ("" + value + (cssNumber.indexOf(key) > -1 ? '' : 'px'))
                      : value;
                  break;
          }
      }
      function get(element, key) {
          switch (nameIndex) {
              // attr
              case 0:
                  // 属性不存在时，原生 getAttribute 方法返回 null，而 jquery 返回 undefined。这里和 jquery 保持一致
                  var value = element.getAttribute(key);
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
          var this$1 = this;

          if (isObjectLike(key)) {
              each(key, function (k, v) {
                  // @ts-ignore
                  this$1[name](k, v);
              });
              return this;
          }
          if (arguments.length === 1) {
              var element = this[0];
              return isElement(element) ? get(element, key) : undefined;
          }
          return this.each(function (i, element) {
              set(element, key, isFunction(value) ? value.call(element, i, get(element, key)) : value);
          });
      };
  });

  $.fn.children = function (selector) {
      var children = [];
      this.each(function (_, element) {
          each(element.childNodes, function (__, childNode) {
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

  $.fn.slice = function () {
      var args = [], len = arguments.length;
      while ( len-- ) args[ len ] = arguments[ len ];

      return new JQ([].slice.apply(this, args));
  };

  $.fn.eq = function (index) {
      var ret = index === -1 ? this.slice(index) : this.slice(index, +index + 1);
      return new JQ(ret);
  };

  function dir($elements, nameIndex, node, selector, filter) {
      var ret = [];
      var target;
      $elements.each(function (_, element) {
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

  each(['', 's', 'sUntil'], function (nameIndex, name) {
      $.fn[("parent" + name)] = function (selector, filter) {
          // parents、parentsUntil 需要把元素的顺序反向处理，以便和 jQuery 的结果一致
          var $nodes = !nameIndex ? this : $(this.get().reverse());
          return dir($nodes, nameIndex, 'parentNode', selector, filter);
      };
  });

  $.fn.closest = function (selector) {
      if (this.is(selector)) {
          return this;
      }
      var matched = [];
      this.parents().each(function (_, element) {
          if ($(element).is(selector)) {
              matched.push(element);
              return false;
          }
      });
      return new JQ(matched);
  };

  var rbrace = /^(?:{[\w\W]*\}|\[[\w\W]*\])$/;
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
          var name = 'data-' + toKebabCase(key);
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
          var element = this[0];
          var resultData = data(element);
          // window, document 上不存在 `data-*` 属性
          if (element.nodeType !== 1) {
              return resultData;
          }
          // 从 `data-*` 中获取值
          var attrs = element.attributes;
          var i = attrs.length;
          while (i--) {
              if (attrs[i]) {
                  var name = attrs[i].name;
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
      each(obj, function (prop, value) {
          // 在 JQ 对象上扩展方法时，需要自己添加 typescript 的类型定义
          $.fn[prop] = value;
      });
      return this;
  };

  $.fn.filter = function (selector) {
      if (isFunction(selector)) {
          return this.map(function (index, element) { return selector.call(element, index, element) ? element : undefined; });
      }
      if (isString(selector)) {
          return this.map(function (_, element) { return $(element).is(selector) ? element : undefined; });
      }
      var $selector = $(selector);
      return this.map(function (_, element) { return $selector.get().indexOf(element) > -1 ? element : undefined; });
  };

  $.fn.first = function () {
      return this.eq(0);
  };

  $.fn.has = function (selector) {
      var $targets = isString(selector) ? this.find(selector) : $(selector);
      var length = $targets.length;
      return this.map(function () {
          for (var i = 0; i < length; i += 1) {
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
      var getExtraWidthValue = function (extra) {
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
      var clientProp = "client" + name;
      var scrollProp = "scroll" + name;
      var offsetProp = "offset" + name;
      var innerProp = "inner" + name;
      // $(window).width()
      if (isWindow(element)) {
          // outerWidth, outerHeight 需要包含滚动条的宽度
          return funcIndex === 2
              ? element[innerProp]
              : toElement(document)[clientProp];
      }
      // $(document).width()
      if (isDocument(element)) {
          var doc = toElement(element);
          return Math.max(
          // @ts-ignore
          element.body[scrollProp], doc[scrollProp], 
          // @ts-ignore
          element.body[offsetProp], doc[offsetProp], doc[clientProp]);
      }
      var value = parseFloat(getComputedStyleValue(element, name.toLowerCase()) || '0');
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
      var computedValue = isFunction(value)
          ? value.call(element, elementIndex, get(element, name, funcIndex, includeMargin))
          : value;
      if (computedValue == null) {
          return;
      }
      var $element = $(element);
      var dimension = name.toLowerCase();
      // 特殊的值，不需要计算 padding、border、margin
      if (['auto', 'inherit', ''].indexOf(computedValue) > -1) {
          $element.css(dimension, computedValue);
          return;
      }
      // 其他值保留原始单位。注意：如果不使用 px 作为单位，则算出的值一般是不准确的
      var suffix = computedValue.toString().replace(/\b[0-9.]*/, '');
      var numerical = parseFloat(computedValue);
      computedValue =
          handleExtraWidth(element, name, numerical, funcIndex, includeMargin, -1) +
              (suffix || 'px');
      $element.css(dimension, computedValue);
  }
  each(['Width', 'Height'], function (_, name) {
      each([("inner" + name), name.toLowerCase(), ("outer" + name)], function (funcIndex, funcName) {
          $.fn[funcName] = function (margin, value) {
              // 是否是赋值操作
              var isSet = arguments.length && (funcIndex < 2 || !isBoolean(margin));
              var includeMargin = margin === true || value === true;
              // 获取第一个元素的值
              if (!isSet) {
                  return this.length
                      ? get(this[0], name, funcIndex, includeMargin)
                      : undefined;
              }
              // 设置每个元素的值
              return this.each(function (index, element) { return set(element, index, name, funcIndex, includeMargin, margin); });
          };
      });
  });

  $.fn.hide = function () {
      return this.each(function () {
          this.style.display = 'none';
      });
  };

  each(['val', 'html', 'text'], function (nameIndex, name) {
      var props = {
          0: 'value',
          1: 'innerHTML',
          2: 'textContent',
      };
      var propName = props[nameIndex];
      function get($elements) {
          // text() 获取所有元素的文本
          if (nameIndex === 2) {
              // @ts-ignore
              return map($elements, function (element) { return toElement(element)[propName]; }).join('');
          }
          // 空集合时，val() 和 html() 返回 undefined
          if (!$elements.length) {
              return undefined;
          }
          // val() 和 html() 仅获取第一个元素的内容
          var firstElement = $elements[0];
          // select multiple 返回数组
          if (nameIndex === 0 && $(firstElement).is('select[multiple]')) {
              return map($(firstElement).find('option:checked'), function (element) { return element.value; });
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
          return this.each(function (i, element) {
              var computedValue = isFunction(value)
                  ? value.call(element, i, get($(element)))
                  : value;
              // value 是数组，则选中数组中的元素，反选不在数组中的元素
              if (nameIndex === 0 && Array.isArray(computedValue)) {
                  // select[multiple]
                  if ($(element).is('select[multiple]')) {
                      map($(element).find('option'), function (option) { return (option.selected =
                          computedValue.indexOf(option.value) >
                              -1); });
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

  each(['', 'All', 'Until'], function (nameIndex, name) {
      $.fn[("next" + name)] = function (selector, filter) {
          return dir(this, nameIndex, 'nextElementSibling', selector, filter);
      };
  });

  $.fn.not = function (selector) {
      var $excludes = this.filter(selector);
      return this.map(function (_, element) { return $excludes.index(element) > -1 ? undefined : element; });
  };

  /**
   * 返回最近的用于定位的父元素
   */
  $.fn.offsetParent = function () {
      return this.map(function () {
          var offsetParent = this.offsetParent;
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
      var $element = this.eq(0);
      var currentOffset;
      var parentOffset = {
          left: 0,
          top: 0,
      };
      if ($element.css('position') === 'fixed') {
          currentOffset = $element[0].getBoundingClientRect();
      }
      else {
          currentOffset = $element.offset();
          var $offsetParent = $element.offsetParent();
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
      var rect = element.getBoundingClientRect();
      var win = element.ownerDocument.defaultView;
      return {
          top: rect.top + win.pageYOffset,
          left: rect.left + win.pageXOffset,
      };
  }
  function set$1(element, value, index) {
      var $element = $(element);
      var position = $element.css('position');
      if (position === 'static') {
          $element.css('position', 'relative');
      }
      var currentOffset = get$1(element);
      var currentTopString = $element.css('top');
      var currentLeftString = $element.css('left');
      var currentTop;
      var currentLeft;
      var calculatePosition = (position === 'absolute' || position === 'fixed') &&
          (currentTopString + currentLeftString).indexOf('auto') > -1;
      if (calculatePosition) {
          var currentPosition = $element.position();
          currentTop = currentPosition.top;
          currentLeft = currentPosition.left;
      }
      else {
          currentTop = parseFloat(currentTopString);
          currentLeft = parseFloat(currentLeftString);
      }
      var computedValue = isFunction(value)
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

  each(['', 'All', 'Until'], function (nameIndex, name) {
      $.fn[("prev" + name)] = function (selector, filter) {
          // prevAll、prevUntil 需要把元素的顺序倒序处理，以便和 jQuery 的结果一致
          var $nodes = !nameIndex ? this : $(this.get().reverse());
          return dir($nodes, nameIndex, 'previousElementSibling', selector, filter);
      };
  });

  $.fn.removeAttr = function (attributeName) {
      var names = attributeName.split(' ').filter(function (name) { return name; });
      return this.each(function () {
          var this$1 = this;

          each(names, function (_, name) {
              this$1.removeAttribute(name);
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
      this.each(function (index, element) {
          var content = newContent;
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
      var this$1 = this;

      return $(target).map(function (index, element) {
          $(element).replaceWith(index ? this$1.clone() : this$1);
          return this$1.get();
      });
  };

  /**
   * 将表单元素的值组合成键值对数组
   * @returns {Array}
   */
  $.fn.serializeArray = function () {
      var result = [];
      this.each(function (_, element) {
          var elements = element instanceof HTMLFormElement ? element.elements : [element];
          $(elements).each(function (_, element) {
              var $element = $(element);
              var type = element.type;
              var nodeName = element.nodeName.toLowerCase();
              if (nodeName !== 'fieldset' &&
                  element.name &&
                  !element.disabled &&
                  ['input', 'select', 'textarea', 'keygen'].indexOf(nodeName) > -1 &&
                  ['submit', 'button', 'image', 'reset', 'file'].indexOf(type) === -1 &&
                  (['radio', 'checkbox'].indexOf(type) === -1 ||
                      element.checked)) {
                  var value = $element.val();
                  var valueArr = Array.isArray(value) ? value : [value];
                  valueArr.forEach(function (value) {
                      result.push({
                          name: element.name,
                          value: value,
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

  var elementDisplay = {};
  /**
   * 获取元素的初始 display 值，用于 .show() 方法
   * @param nodeName
   */
  function defaultDisplay(nodeName) {
      var element;
      var display;
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
          duration = duration + "ms";
      }
      return this.each(function () {
          this.style.webkitTransitionDuration = duration;
          this.style.transitionDuration = duration;
      });
  };

  $.fn.transitionEnd = function (callback) {
      // eslint-disable-next-line @typescript-eslint/no-this-alias
      var that = this;
      var events = ['webkitTransitionEnd', 'transitionend'];
      function fireCallback(e) {
          if (e.target !== this) {
              return;
          }
          // @ts-ignore
          callback.call(this, e);
          each(events, function (_, event) {
              that.off(event, fireCallback);
          });
      }
      each(events, function (_, event) {
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
  var entries = {};
  /**
   * 注册并执行初始化函数
   * @param selector CSS 选择器
   * @param apiInit 初始化函数
   * @param i 元素索引
   * @param element 元素
   */
  function mutation(selector, apiInit, i, element) {
      var selectors = data(element, '_mdui_mutation');
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
      return this.each(function (i, element) {
          var $this = $(element);
          each(entries, function (selector, apiInit) {
              if ($this.is(selector)) {
                  mutation(selector, apiInit, i, element);
              }
              $this.find(selector).each(function (i, element) {
                  mutation(selector, apiInit, i, element);
              });
          });
      });
  };

  $.showOverlay = function (zIndex) {
      var $overlay = $('.mdui-overlay');
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
      var level = $overlay.data('_overlay_level') || 0;
      return $overlay.data('_overlay_level', ++level).addClass('mdui-overlay-show');
  };

  $.hideOverlay = function (force) {
      if ( force === void 0 ) force = false;

      var $overlay = $('.mdui-overlay');
      if (!$overlay.length) {
          return;
      }
      var level = force ? 1 : $overlay.data('_overlay_level');
      if (level > 1) {
          $overlay.data('_overlay_level', --level);
          return;
      }
      $overlay
          .data('_overlay_level', 0)
          .removeClass('mdui-overlay-show')
          .data('_overlay_is_deleted', true)
          .transitionEnd(function () {
          if ($overlay.data('_overlay_is_deleted')) {
              $overlay.remove();
          }
      });
  };

  $.lockScreen = function () {
      var $body = $('body');
      // 不直接把 body 设为 box-sizing: border-box，避免污染全局样式
      var newBodyWidth = $body.width();
      var level = $body.data('_lockscreen_level') || 0;
      $body
          .addClass('mdui-locked')
          .width(newBodyWidth)
          .data('_lockscreen_level', ++level);
  };

  $.unlockScreen = function (force) {
      if ( force === void 0 ) force = false;

      var $body = $('body');
      var level = force ? 1 : $body.data('_lockscreen_level');
      if (level > 1) {
          $body.data('_lockscreen_level', --level);
          return;
      }
      $body.data('_lockscreen_level', 0).removeClass('mdui-locked').width('');
  };

  $.throttle = function (fn, delay) {
      if ( delay === void 0 ) delay = 16;

      var timer = null;
      return function () {
          var this$1 = this;
          var args = [], len = arguments.length;
          while ( len-- ) args[ len ] = arguments[ len ];

          if (isNull(timer)) {
              timer = setTimeout(function () {
                  fn.apply(this$1, args);
                  timer = null;
              }, delay);
          }
      };
  };

  var GUID = {};
  $.guid = function (name) {
      if (!isUndefined(name) && !isUndefined(GUID[name])) {
          return GUID[name];
      }
      function s4() {
          return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
      }
      var guid = '_' +
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
      $(selector).each(function (i, element) { return mutation(selector, apiInit, i, element); });
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
      var fullEventName = eventName + ".mdui." + componentName;
      // jQuery 事件
      // @ts-ignore
      if (typeof jQuery !== 'undefined') {
          // @ts-ignore
          jQuery(target).trigger(fullEventName, parameters);
      }
      var $target = $(target);
      // mdui.jq 事件
      $target.trigger(fullEventName, parameters);
      var eventParams = {
          bubbles: true,
          cancelable: true,
          detail: parameters,
      };
      var eventObject = new CustomEvent(fullEventName, eventParams);
      // @ts-ignore
      eventObject._detail = parameters;
      $target[0].dispatchEvent(eventObject);
  }

  var $document = $(document);
  var $window = $(window);
  $('body');

  var DEFAULT_OPTIONS = {
      tolerance: 5,
      offset: 0,
      initialClass: 'mdui-headroom',
      pinnedClass: 'mdui-headroom-pinned-top',
      unpinnedClass: 'mdui-headroom-unpinned-top',
  };
  var Headroom = function Headroom(selector, options) {
      if ( options === void 0 ) options = {};

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
      var tolerance = this.options.tolerance;
      if (isNumber(tolerance)) {
          this.options.tolerance = {
              down: tolerance,
              up: tolerance,
          };
      }
      this.enable();
  };
  /**
   * 滚动时的处理
   */
  Headroom.prototype.onScroll = function onScroll () {
          var this$1 = this;

      this.rafId = window.requestAnimationFrame(function () {
          var currentScrollY = window.pageYOffset;
          var direction = currentScrollY > this$1.lastScrollY ? 'down' : 'up';
          var tolerance = this$1.options.tolerance[direction];
          var scrolled = Math.abs(currentScrollY - this$1.lastScrollY);
          var toleranceExceeded = scrolled >= tolerance;
          if (currentScrollY > this$1.lastScrollY &&
              currentScrollY >= this$1.options.offset &&
              toleranceExceeded) {
              this$1.unpin();
          }
          else if ((currentScrollY < this$1.lastScrollY && toleranceExceeded) ||
              currentScrollY <= this$1.options.offset) {
              this$1.pin();
          }
          this$1.lastScrollY = currentScrollY;
      });
  };
  /**
   * 触发组件事件
   * @param name
   */
  Headroom.prototype.triggerEvent = function triggerEvent (name) {
      componentEvent(name, 'headroom', this.$element, this);
  };
  /**
   * 动画结束的回调
   */
  Headroom.prototype.transitionEnd = function transitionEnd () {
      if (this.state === 'pinning') {
          this.state = 'pinned';
          this.triggerEvent('pinned');
      }
      if (this.state === 'unpinning') {
          this.state = 'unpinned';
          this.triggerEvent('unpinned');
      }
  };
  /**
   * 使元素固定住
   */
  Headroom.prototype.pin = function pin () {
          var this$1 = this;

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
          .transitionEnd(function () { return this$1.transitionEnd(); });
  };
  /**
   * 使元素隐藏
   */
  Headroom.prototype.unpin = function unpin () {
          var this$1 = this;

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
          .transitionEnd(function () { return this$1.transitionEnd(); });
  };
  /**
   * 启用 headroom 插件
   */
  Headroom.prototype.enable = function enable () {
          var this$1 = this;

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
      $window.on('scroll', function () { return this$1.onScroll(); });
  };
  /**
   * 禁用 headroom 插件
   */
  Headroom.prototype.disable = function disable () {
          var this$1 = this;

      if (!this.isEnable) {
          return;
      }
      this.isEnable = false;
      this.$element
          .removeClass(this.options.initialClass)
          .removeClass(this.options.pinnedClass)
          .removeClass(this.options.unpinnedClass);
      $window.off('scroll', function () { return this$1.onScroll(); });
      window.cancelAnimationFrame(this.rafId);
  };
  /**
   * 获取当前状态。共包含四种状态：`pinning`、`pinned`、`unpinning`、`unpinned`
   */
  Headroom.prototype.getState = function getState () {
      return this.state;
  };
  mdui.Headroom = Headroom;

  /**
   * 解析 DATA API 参数
   * @param element 元素
   * @param name 属性名
   */
  function parseOptions(element, name) {
      var attr = $(element).attr(name);
      if (!attr) {
          return {};
      }
      return new Function('', ("var json = " + attr + "; return JSON.parse(JSON.stringify(json));"))();
  }

  var customAttr = 'mdui-headroom';
  $(function () {
      mdui.mutation(("[" + customAttr + "]"), function () {
          new mdui.Headroom(this, parseOptions(this, customAttr));
      });
  });

  var DEFAULT_OPTIONS$1 = {
      accordion: false,
  };
  var CollapseAbstract = function CollapseAbstract(selector, options) {
      if ( options === void 0 ) options = {};

      /**
       * 配置参数
       */
      this.options = extend({}, DEFAULT_OPTIONS$1);
      // CSS 类名
      var classPrefix = "mdui-" + (this.getNamespace()) + "-item";
      this.classItem = classPrefix;
      this.classItemOpen = classPrefix + "-open";
      this.classHeader = classPrefix + "-header";
      this.classBody = classPrefix + "-body";
      this.$element = $(selector).first();
      extend(this.options, options);
      this.bindEvent();
  };
  /**
   * 绑定事件
   */
  CollapseAbstract.prototype.bindEvent = function bindEvent () {
      // eslint-disable-next-line @typescript-eslint/no-this-alias
      var that = this;
      // 点击 header 时，打开/关闭 item
      this.$element.on('click', ("." + (this.classHeader)), function () {
          var $header = $(this);
          var $item = $header.parent();
          var $items = that.getItems();
          $items.each(function (_, item) {
              if ($item.is(item)) {
                  that.toggle(item);
              }
          });
      });
      // 点击关闭按钮时，关闭 item
      this.$element.on('click', ("[mdui-" + (this.getNamespace()) + "-item-close]"), function () {
          var $target = $(this);
          var $item = $target.parents(("." + (that.classItem))).first();
          that.close($item);
      });
  };
  /**
   * 指定 item 是否处于打开状态
   * @param $item
   */
  CollapseAbstract.prototype.isOpen = function isOpen ($item) {
      return $item.hasClass(this.classItemOpen);
  };
  /**
   * 获取所有 item
   */
  CollapseAbstract.prototype.getItems = function getItems () {
      return this.$element.children(("." + (this.classItem)));
  };
  /**
   * 获取指定 item
   * @param item
   */
  CollapseAbstract.prototype.getItem = function getItem (item) {
      if (isNumber(item)) {
          return this.getItems().eq(item);
      }
      return $(item).first();
  };
  /**
   * 触发组件事件
   * @param name 事件名
   * @param $item 事件触发的目标 item
   */
  CollapseAbstract.prototype.triggerEvent = function triggerEvent (name, $item) {
      componentEvent(name, this.getNamespace(), $item, this);
  };
  /**
   * 动画结束回调
   * @param $content body 元素
   * @param $item item 元素
   */
  CollapseAbstract.prototype.transitionEnd = function transitionEnd ($content, $item) {
      if (this.isOpen($item)) {
          $content.transition(0).height('auto').reflow().transition('');
          this.triggerEvent('opened', $item);
      }
      else {
          $content.height('');
          this.triggerEvent('closed', $item);
      }
  };
  /**
   * 打开指定面板项
   * @param item 面板项的索引号、或 CSS 选择器、或 DOM 元素、或 JQ 对象
   */
  CollapseAbstract.prototype.open = function open (item) {
          var this$1 = this;

      var $item = this.getItem(item);
      if (this.isOpen($item)) {
          return;
      }
      // 关闭其他项
      if (this.options.accordion) {
          this.$element.children(("." + (this.classItemOpen))).each(function (_, element) {
              var $element = $(element);
              if (!$element.is($item)) {
                  this$1.close($element);
              }
          });
      }
      var $content = $item.children(("." + (this.classBody)));
      $content
          .height($content[0].scrollHeight)
          .transitionEnd(function () { return this$1.transitionEnd($content, $item); });
      this.triggerEvent('open', $item);
      $item.addClass(this.classItemOpen);
  };
  /**
   * 关闭指定面板项
   * @param item 面板项的索引号、或 CSS 选择器、或 DOM 元素、或 JQ 对象
   */
  CollapseAbstract.prototype.close = function close (item) {
          var this$1 = this;

      var $item = this.getItem(item);
      if (!this.isOpen($item)) {
          return;
      }
      var $content = $item.children(("." + (this.classBody)));
      this.triggerEvent('close', $item);
      $item.removeClass(this.classItemOpen);
      $content
          .transition(0)
          .height($content[0].scrollHeight)
          .reflow()
          .transition('')
          .height('')
          .transitionEnd(function () { return this$1.transitionEnd($content, $item); });
  };
  /**
   * 切换指定面板项的打开状态
   * @param item 面板项的索引号、或 CSS 选择器、或 DOM 元素、或 JQ 对象
   */
  CollapseAbstract.prototype.toggle = function toggle (item) {
      var $item = this.getItem(item);
      this.isOpen($item) ? this.close($item) : this.open($item);
  };
  /**
   * 打开所有面板项
   */
  CollapseAbstract.prototype.openAll = function openAll () {
          var this$1 = this;

      this.getItems().each(function (_, element) { return this$1.open(element); });
  };
  /**
   * 关闭所有面板项
   */
  CollapseAbstract.prototype.closeAll = function closeAll () {
          var this$1 = this;

      this.getItems().each(function (_, element) { return this$1.close(element); });
  };

  var Collapse = /*@__PURE__*/(function (CollapseAbstract) {
      function Collapse () {
          CollapseAbstract.apply(this, arguments);
      }

      if ( CollapseAbstract ) Collapse.__proto__ = CollapseAbstract;
      Collapse.prototype = Object.create( CollapseAbstract && CollapseAbstract.prototype );
      Collapse.prototype.constructor = Collapse;

      Collapse.prototype.getNamespace = function getNamespace () {
          return 'collapse';
      };

      return Collapse;
  }(CollapseAbstract));
  mdui.Collapse = Collapse;

  var customAttr$1 = 'mdui-collapse';
  $(function () {
      mdui.mutation(("[" + customAttr$1 + "]"), function () {
          new mdui.Collapse(this, parseOptions(this, customAttr$1));
      });
  });

  var DEFAULT_OPTIONS$2 = {
      trigger: 'click',
      loop: false,
  };
  var Tab = function Tab(selector, options) {
      var this$1 = this;
      if ( options === void 0 ) options = {};

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
      var hash = window.location.hash;
      if (hash) {
          this.$tabs.each(function (index, tab) {
              if ($(tab).attr('href') === hash) {
                  this$1.activeIndex = index;
                  return false;
              }
              return true;
          });
      }
      // 含 .mdui-tab-active 的元素默认激活
      if (this.activeIndex === -1) {
          this.$tabs.each(function (index, tab) {
              if ($(tab).hasClass('mdui-tab-active')) {
                  this$1.activeIndex = index;
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
      $window.on('resize', $.throttle(function () { return this$1.setIndicatorPosition(); }, 100));
      // 监听点击选项卡事件
      this.$tabs.each(function (_, tab) {
          this$1.bindTabEvent(tab);
      });
  };
  /**
   * 指定选项卡是否已禁用
   * @param $tab
   */
  Tab.prototype.isDisabled = function isDisabled ($tab) {
      return $tab.attr('disabled') !== undefined;
  };
  /**
   * 绑定在 Tab 上点击或悬浮的事件
   * @param tab
   */
  Tab.prototype.bindTabEvent = function bindTabEvent (tab) {
          var this$1 = this;

      var $tab = $(tab);
      // 点击或鼠标移入触发的事件
      var clickEvent = function () {
          // 禁用状态的选项卡无法选中
          if (this$1.isDisabled($tab)) {
              return false;
          }
          this$1.activeIndex = this$1.$tabs.index(tab);
          this$1.setActive();
      };
      // 无论 trigger 是 click 还是 hover，都会响应 click 事件
      $tab.on('click', clickEvent);
      // trigger 为 hover 时，额外响应 mouseenter 事件
      if (this.options.trigger === 'hover') {
          $tab.on('mouseenter', clickEvent);
      }
      // 阻止链接的默认点击动作
      $tab.on('click', function () {
          if (($tab.attr('href') || '').indexOf('#') === 0) {
              return false;
          }
      });
  };
  /**
   * 触发组件事件
   * @param name
   * @param $element
   * @param parameters
   */
  Tab.prototype.triggerEvent = function triggerEvent (name, $element, parameters) {
          if ( parameters === void 0 ) parameters = {};

      componentEvent(name, 'tab', $element, this, parameters);
  };
  /**
   * 设置激活状态的选项卡
   */
  Tab.prototype.setActive = function setActive () {
          var this$1 = this;

      this.$tabs.each(function (index, tab) {
          var $tab = $(tab);
          var targetId = $tab.attr('href') || '';
          // 设置选项卡激活状态
          if (index === this$1.activeIndex && !this$1.isDisabled($tab)) {
              if (!$tab.hasClass('mdui-tab-active')) {
                  this$1.triggerEvent('change', this$1.$element, {
                      index: this$1.activeIndex,
                      id: targetId.substr(1),
                  });
                  this$1.triggerEvent('show', $tab);
                  $tab.addClass('mdui-tab-active');
              }
              $(targetId).show();
              this$1.setIndicatorPosition();
          }
          else {
              $tab.removeClass('mdui-tab-active');
              $(targetId).hide();
          }
      });
  };
  /**
   * 设置选项卡指示器的位置
   */
  Tab.prototype.setIndicatorPosition = function setIndicatorPosition () {
      // 选项卡数量为 0 时，不显示指示器
      if (this.activeIndex === -1) {
          this.$indicator.css({
              left: 0,
              width: 0,
          });
          return;
      }
      var $activeTab = this.$tabs.eq(this.activeIndex);
      if (this.isDisabled($activeTab)) {
          return;
      }
      var activeTabOffset = $activeTab.offset();
      this.$indicator.css({
          left: ((activeTabOffset.left +
              this.$element[0].scrollLeft -
              this.$element[0].getBoundingClientRect().left) + "px"),
          width: (($activeTab.innerWidth()) + "px"),
      });
  };
  /**
   * 切换到下一个选项卡
   */
  Tab.prototype.next = function next () {
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
  };
  /**
   * 切换到上一个选项卡
   */
  Tab.prototype.prev = function prev () {
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
  };
  /**
   * 显示指定索引号、或指定id的选项卡
   * @param index 索引号、或id
   */
  Tab.prototype.show = function show (index) {
          var this$1 = this;

      if (this.activeIndex === -1) {
          return;
      }
      if (isNumber(index)) {
          this.activeIndex = index;
      }
      else {
          this.$tabs.each(function (i, tab) {
              if (tab.id === index) {
                  this$1.activeIndex = i;
                  return false;
              }
          });
      }
      this.setActive();
  };
  /**
   * 在父元素的宽度变化时，需要调用该方法重新调整指示器位置
   * 在添加或删除选项卡时，需要调用该方法
   */
  Tab.prototype.handleUpdate = function handleUpdate () {
          var this$1 = this;

      var $oldTabs = this.$tabs; // 旧的 tabs JQ对象
      var $newTabs = this.$element.children('a'); // 新的 tabs JQ对象
      var oldTabsElement = $oldTabs.get(); // 旧的 tabs 元素数组
      var newTabsElement = $newTabs.get(); // 新的 tabs 元素数组
      if (!$newTabs.length) {
          this.activeIndex = -1;
          this.$tabs = $newTabs;
          this.setIndicatorPosition();
          return;
      }
      // 重新遍历选项卡，找出新增的选项卡
      $newTabs.each(function (index, tab) {
          // 有新增的选项卡
          if (oldTabsElement.indexOf(tab) < 0) {
              this$1.bindTabEvent(tab);
              if (this$1.activeIndex === -1) {
                  this$1.activeIndex = 0;
              }
              else if (index <= this$1.activeIndex) {
                  this$1.activeIndex++;
              }
          }
      });
      // 找出被移除的选项卡
      $oldTabs.each(function (index, tab) {
          // 有被移除的选项卡
          if (newTabsElement.indexOf(tab) < 0) {
              if (index < this$1.activeIndex) {
                  this$1.activeIndex--;
              }
              else if (index === this$1.activeIndex) {
                  this$1.activeIndex = 0;
              }
          }
      });
      this.$tabs = $newTabs;
      this.setActive();
  };
  mdui.Tab = Tab;

  var customAttr$2 = 'mdui-tab';
  $(function () {
      mdui.mutation(("[" + customAttr$2 + "]"), function () {
          new mdui.Tab(this, parseOptions(this, customAttr$2));
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
  var startEvent = 'touchstart mousedown';
  var unlockEvent = 'touchend touchmove touchcancel';
  var touches = 0;
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
              'mouseleave' ].indexOf(event.type) > -1);
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

  var DEFAULT_OPTIONS$3 = {
      position: 'auto',
      delay: 0,
      content: '',
  };
  var Tooltip = function Tooltip(selector, options) {
      if ( options === void 0 ) options = {};

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
      this.$element = $(("<div class=\"mdui-tooltip\" id=\"" + ($.guid()) + "\">" + (this.options.content) + "</div>")).appendTo(document.body);
      // 绑定事件。元素处于 disabled 状态时无法触发鼠标事件，为了统一，把 touch 事件也禁用
      // eslint-disable-next-line @typescript-eslint/no-this-alias
      var that = this;
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
  };
  /**
   * 元素是否已禁用
   * @param element
   */
  Tooltip.prototype.isDisabled = function isDisabled (element) {
      return (element.disabled ||
          $(element).attr('disabled') !== undefined);
  };
  /**
   * 是否是桌面设备
   */
  Tooltip.prototype.isDesktop = function isDesktop () {
      return $window.width() > 1024;
  };
  /**
   * 设置 Tooltip 的位置
   */
  Tooltip.prototype.setPosition = function setPosition () {
      var marginLeft;
      var marginTop;
      // 触发的元素
      var targetProps = this.$target[0].getBoundingClientRect();
      // 触发的元素和 Tooltip 之间的距离
      var targetMargin = this.isDesktop() ? 14 : 24;
      // Tooltip 的宽度和高度
      var tooltipWidth = this.$element[0].offsetWidth;
      var tooltipHeight = this.$element[0].offsetHeight;
      // Tooltip 的方向
      var position = this.options.position;
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
      var targetOffset = this.$target.offset();
      this.$element.css({
          top: ((targetOffset.top + targetProps.height / 2) + "px"),
          left: ((targetOffset.left + targetProps.width / 2) + "px"),
          'margin-left': (marginLeft + "px"),
          'margin-top': (marginTop + "px"),
      });
  };
  /**
   * 触发组件事件
   * @param name
   */
  Tooltip.prototype.triggerEvent = function triggerEvent (name) {
      componentEvent(name, 'tooltip', this.$target, this);
  };
  /**
   * 动画结束回调
   */
  Tooltip.prototype.transitionEnd = function transitionEnd () {
      if (this.$element.hasClass('mdui-tooltip-open')) {
          this.state = 'opened';
          this.triggerEvent('opened');
      }
      else {
          this.state = 'closed';
          this.triggerEvent('closed');
      }
  };
  /**
   * 当前 tooltip 是否为打开状态
   */
  Tooltip.prototype.isOpen = function isOpen () {
      return this.state === 'opening' || this.state === 'opened';
  };
  /**
   * 执行打开 tooltip
   */
  Tooltip.prototype.doOpen = function doOpen () {
          var this$1 = this;

      this.state = 'opening';
      this.triggerEvent('open');
      this.$element
          .addClass('mdui-tooltip-open')
          .transitionEnd(function () { return this$1.transitionEnd(); });
  };
  /**
   * 打开 Tooltip
   * @param options 允许每次打开时设置不同的参数
   */
  Tooltip.prototype.open = function open (options) {
          var this$1 = this;

      if (this.isOpen()) {
          return;
      }
      var oldOptions = extend({}, this.options);
      if (options) {
          extend(this.options, options);
      }
      // tooltip 的内容有更新
      if (oldOptions.content !== this.options.content) {
          this.$element.html(this.options.content);
      }
      this.setPosition();
      if (this.options.delay) {
          this.timeoutId = setTimeout(function () { return this$1.doOpen(); }, this.options.delay);
      }
      else {
          this.timeoutId = null;
          this.doOpen();
      }
  };
  /**
   * 关闭 Tooltip
   */
  Tooltip.prototype.close = function close () {
          var this$1 = this;

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
          .transitionEnd(function () { return this$1.transitionEnd(); });
  };
  /**
   * 切换 Tooltip 的打开状态
   */
  Tooltip.prototype.toggle = function toggle () {
      this.isOpen() ? this.close() : this.open();
  };
  /**
   * 获取 Tooltip 状态。共包含四种状态：`opening`、`opened`、`closing`、`closed`
   */
  Tooltip.prototype.getState = function getState () {
      return this.state;
  };
  mdui.Tooltip = Tooltip;

  var customAttr$3 = 'mdui-tooltip';
  var dataName = '_mdui_tooltip';
  $(function () {
      // mouseenter 不能冒泡，所以这里用 mouseover 代替
      $document.on('touchstart mouseover', ("[" + customAttr$3 + "]"), function () {
          var $target = $(this);
          var instance = $target.data(dataName);
          if (!instance) {
              instance = new mdui.Tooltip(this, parseOptions(this, customAttr$3));
              $target.data(dataName, instance);
          }
      });
  });

  var container = {};
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
      var func = container[name].shift();
      func();
  }

  var DEFAULT_OPTIONS$4 = {
      message: '',
      timeout: 4000,
      position: 'bottom',
      buttonText: '',
      buttonColor: '',
      closeOnButtonClick: true,
      closeOnOutsideClick: true,
      // eslint-disable-next-line @typescript-eslint/no-empty-function
      onClick: function () { },
      // eslint-disable-next-line @typescript-eslint/no-empty-function
      onButtonClick: function () { },
      // eslint-disable-next-line @typescript-eslint/no-empty-function
      onOpen: function () { },
      // eslint-disable-next-line @typescript-eslint/no-empty-function
      onOpened: function () { },
      // eslint-disable-next-line @typescript-eslint/no-empty-function
      onClose: function () { },
      // eslint-disable-next-line @typescript-eslint/no-empty-function
      onClosed: function () { },
  };
  /**
   * 当前打开着的 Snackbar
   */
  var currentInst = null;
  /**
   * 队列名
   */
  var queueName = '_mdui_snackbar';
  var Snackbar = function Snackbar(options) {
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
      var buttonColorStyle = '';
      var buttonColorClass = '';
      if (this.options.buttonColor.indexOf('#') === 0 ||
          this.options.buttonColor.indexOf('rgb') === 0) {
          buttonColorStyle = "style=\"color:" + (this.options.buttonColor) + "\"";
      }
      else if (this.options.buttonColor !== '') {
          buttonColorClass = "mdui-text-color-" + (this.options.buttonColor);
      }
      // 添加 HTML
      this.$element = $('<div class="mdui-snackbar">' +
          "<div class=\"mdui-snackbar-text\">" + (this.options.message) + "</div>" +
          (this.options.buttonText
              ? ("<a href=\"javascript:void(0)\" class=\"mdui-snackbar-action mdui-btn mdui-ripple mdui-ripple-white " + buttonColorClass + "\" " + buttonColorStyle + ">" + (this.options.buttonText) + "</a>")
              : '') +
          '</div>').appendTo(document.body);
      // 设置位置
      this.setPosition('close');
      this.$element.reflow().addClass(("mdui-snackbar-" + (this.options.position)));
  };
  /**
   * 点击 Snackbar 外面的区域关闭
   * @param event
   */
  Snackbar.prototype.closeOnOutsideClick = function closeOnOutsideClick (event) {
      var $target = $(event.target);
      if (!$target.hasClass('mdui-snackbar') &&
          !$target.parents('.mdui-snackbar').length) {
          currentInst.close();
      }
  };
  /**
   * 设置 Snackbar 的位置
   * @param state
   */
  Snackbar.prototype.setPosition = function setPosition (state) {
      var snackbarHeight = this.$element[0].clientHeight;
      var position = this.options.position;
      var translateX;
      var translateY;
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
      this.$element.transform(("translate(" + translateX + "," + translateY + "px"));
  };
  /**
   * 打开 Snackbar
   */
  Snackbar.prototype.open = function open () {
          var this$1 = this;

      if (this.state === 'opening' || this.state === 'opened') {
          return;
      }
      // 如果当前有正在显示的 Snackbar，则先加入队列，等旧 Snackbar 关闭后再打开
      if (currentInst) {
          queue(queueName, function () { return this$1.open(); });
          return;
      }
      currentInst = this;
      // 开始打开
      this.state = 'opening';
      this.options.onOpen(this);
      this.setPosition('open');
      this.$element.transitionEnd(function () {
          if (this$1.state !== 'opening') {
              return;
          }
          this$1.state = 'opened';
          this$1.options.onOpened(this$1);
          // 有按钮时绑定事件
          if (this$1.options.buttonText) {
              this$1.$element.find('.mdui-snackbar-action').on('click', function () {
                  this$1.options.onButtonClick(this$1);
                  if (this$1.options.closeOnButtonClick) {
                      this$1.close();
                  }
              });
          }
          // 点击 snackbar 的事件
          this$1.$element.on('click', function (event) {
              if (!$(event.target).hasClass('mdui-snackbar-action')) {
                  this$1.options.onClick(this$1);
              }
          });
          // 点击 Snackbar 外面的区域关闭
          if (this$1.options.closeOnOutsideClick) {
              $document.on(startEvent, this$1.closeOnOutsideClick);
          }
          // 超时后自动关闭
          if (this$1.options.timeout) {
              this$1.timeoutId = setTimeout(function () { return this$1.close(); }, this$1.options.timeout);
          }
      });
  };
  /**
   * 关闭 Snackbar
   */
  Snackbar.prototype.close = function close () {
          var this$1 = this;

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
      this.$element.transitionEnd(function () {
          if (this$1.state !== 'closing') {
              return;
          }
          currentInst = null;
          this$1.state = 'closed';
          this$1.options.onClosed(this$1);
          this$1.$element.remove();
          dequeue(queueName);
      });
  };
  mdui.snackbar = function (message, options) {
      if ( options === void 0 ) options = {};

      if (isString(message)) {
          options.message = message;
      }
      else {
          options = message;
      }
      var instance = new Snackbar(options);
      instance.open();
      return instance;
  };

  $(function () {
      // 切换导航项
      $document.on('click', '.mdui-bottom-nav>a', function () {
          var $item = $(this);
          var $bottomNav = $item.parent();
          $bottomNav.children('a').each(function (index, item) {
              var isThis = $item.is(item);
              if (isThis) {
                  componentEvent('change', 'bottomNav', $bottomNav[0], undefined, {
                      index: index,
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

  return mdui;

})));
//# sourceMappingURL=mdui.js.map
