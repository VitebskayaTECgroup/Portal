(function (self, undefined) {
  var _DOMTokenList = (function () {
    var n = !0,
      t = function (t, e, r, o) {
        Object.defineProperty
          ? Object.defineProperty(t, e, {
              configurable: !1 === n || !!o,
              get: r,
            })
          : t.__defineGetter__(e, r);
      };
    try {
      t({}, "support");
    } catch (e) {
      n = !1;
    }
    return function (n, e) {
      var r = this,
        o = [],
        i = {},
        a = 0,
        c = 0,
        f = function (n) {
          t(
            r,
            n,
            function () {
              return u(), o[n];
            },
            !1
          );
        },
        l = function () {
          if (a >= c) for (; c < a; ++c) f(c);
        },
        u = function () {
          var t,
            r,
            c = arguments,
            f = /\s+/;
          if (c.length)
            for (r = 0; r < c.length; ++r)
              if (f.test(c[r]))
                throw (
                  ((t = new SyntaxError(
                    'String "' + c[r] + '" contains an invalid character'
                  )),
                  (t.code = 5),
                  (t.name = "InvalidCharacterError"),
                  t)
                );
          for (
            o =
              "object" == typeof n[e]
                ? ("" + n[e].baseVal).replace(/^\s+|\s+$/g, "").split(f)
                : ("" + n[e]).replace(/^\s+|\s+$/g, "").split(f),
              "" === o[0] && (o = []),
              i = {},
              r = 0;
            r < o.length;
            ++r
          )
            i[o[r]] = !0;
          (a = o.length), l();
        };
      return (
        u(),
        t(r, "length", function () {
          return u(), a;
        }),
        (r.toLocaleString = r.toString =
          function () {
            return u(), o.join(" ");
          }),
        (r.item = function (n) {
          return u(), o[n];
        }),
        (r.contains = function (n) {
          return u(), !!i[n];
        }),
        (r.add = function () {
          u.apply(r, (t = arguments));
          for (var t, c, f = 0, p = t.length; f < p; ++f)
            (c = t[f]), i[c] || (o.push(c), (i[c] = !0));
          a !== o.length &&
            ((a = o.length >>> 0),
            "object" == typeof n[e]
              ? (n[e].baseVal = o.join(" "))
              : (n[e] = o.join(" ")),
            l());
        }),
        (r.remove = function () {
          u.apply(r, (t = arguments));
          for (var t, c = {}, f = 0, p = []; f < t.length; ++f)
            (c[t[f]] = !0), delete i[t[f]];
          for (f = 0; f < o.length; ++f) c[o[f]] || p.push(o[f]);
          (o = p),
            (a = p.length >>> 0),
            "object" == typeof n[e]
              ? (n[e].baseVal = o.join(" "))
              : (n[e] = o.join(" ")),
            l();
        }),
        (r.toggle = function (n, t) {
          return (
            u.apply(r, [n]),
            undefined !== t
              ? t
                ? (r.add(n), !0)
                : (r.remove(n), !1)
              : i[n]
              ? (r.remove(n), !1)
              : (r.add(n), !0)
          );
        }),
        (r.forEach = Array.prototype.forEach),
        r
      );
    };
  })();
  function ArrayCreate(r) {
    if ((1 / r == -Infinity && (r = 0), r > Math.pow(2, 32) - 1))
      throw new RangeError("Invalid array length");
    var n = [];
    return (n.length = r), n;
  }
  function Call(t, l) {
    var n = arguments.length > 2 ? arguments[2] : [];
    if (!1 === IsCallable(t))
      throw new TypeError(
        Object.prototype.toString.call(t) + "is not a function."
      );
    return t.apply(l, n);
  }
  function CreateDataProperty(e, r, t) {
    var a = { value: t, writable: !0, enumerable: !0, configurable: !0 };
    try {
      return Object.defineProperty(e, r, a), !0;
    } catch (n) {
      return !1;
    }
  }
  function CreateDataPropertyOrThrow(t, r, o) {
    var e = CreateDataProperty(t, r, o);
    if (!e)
      throw new TypeError(
        "Cannot assign value `" +
          Object.prototype.toString.call(o) +
          "` to property `" +
          Object.prototype.toString.call(r) +
          "` on object `" +
          Object.prototype.toString.call(t) +
          "`"
      );
    return e;
  }
  function CreateMethodProperty(e, r, t) {
    var a = { value: t, writable: !0, enumerable: !1, configurable: !0 };
    Object.defineProperty(e, r, a);
  }
  function Get(n, t) {
    return n[t];
  }
  function HasOwnProperty(r, t) {
    return Object.prototype.hasOwnProperty.call(r, t);
  }
  function IsCallable(n) {
    return "function" == typeof n;
  }
  function RequireObjectCoercible(e) {
    if (null === e || e === undefined)
      throw TypeError(
        Object.prototype.toString.call(e) + " is not coercible to Object."
      );
    return e;
  }
  function SameValueNonNumber(e, n) {
    return e === n;
  }
  function ToBoolean(o) {
    return Boolean(o);
  }
  function ToObject(e) {
    if (null === e || e === undefined) throw TypeError();
    return Object(e);
  }
  function GetV(t, e) {
    return ToObject(t)[e];
  }
  function GetMethod(e, n) {
    var r = GetV(e, n);
    if (null === r || r === undefined) return undefined;
    if (!1 === IsCallable(r)) throw new TypeError("Method not callable: " + n);
    return r;
  }
  function Type(e) {
    switch (typeof e) {
      case "undefined":
        return "undefined";
      case "boolean":
        return "boolean";
      case "number":
        return "number";
      case "string":
        return "string";
      case "symbol":
        return "symbol";
      default:
        return null === e
          ? "null"
          : "Symbol" in self &&
            (e instanceof self.Symbol || e.constructor === self.Symbol)
          ? "symbol"
          : "object";
    }
  }
  function CreateIterResultObject(e, r) {
    if ("boolean" !== Type(r)) throw new Error();
    var t = {};
    return (
      CreateDataProperty(t, "value", e), CreateDataProperty(t, "done", r), t
    );
  }
  function GetPrototypeFromConstructor(t, o) {
    var r = Get(t, "prototype");
    return "object" !== Type(r) && (r = o), r;
  }
  function OrdinaryCreateFromConstructor(r, e) {
    var t = arguments[2] || {},
      o = GetPrototypeFromConstructor(r, e),
      a = Object.create(o);
    for (var n in t)
      Object.prototype.hasOwnProperty.call(t, n) &&
        Object.defineProperty(a, n, {
          configurable: !0,
          enumerable: !1,
          writable: !0,
          value: t[n],
        });
    return a;
  }
  function IsConstructor(t) {
    return "object" === Type(t) && "function" == typeof t && !!t.prototype;
  }
  function Construct(r) {
    var t = arguments.length > 2 ? arguments[2] : r,
      o = arguments.length > 1 ? arguments[1] : [];
    if (!IsConstructor(r)) throw new TypeError("F must be a constructor.");
    if (!IsConstructor(t))
      throw new TypeError("newTarget must be a constructor.");
    if (t === r)
      return new (Function.prototype.bind.apply(r, [null].concat(o)))();
    var n = OrdinaryCreateFromConstructor(t, Object.prototype);
    return Call(r, n, o);
  }
  function IsRegExp(e) {
    if ("object" !== Type(e)) return !1;
    var n =
      "Symbol" in self && "match" in self.Symbol
        ? Get(e, self.Symbol.match)
        : undefined;
    if (n !== undefined) return ToBoolean(n);
    try {
      var t = e.lastIndex;
      return (e.lastIndex = 0), RegExp.prototype.exec.call(e), !0;
    } catch (l) {
    } finally {
      e.lastIndex = t;
    }
    return !1;
  }
  function IteratorClose(r, t) {
    if ("object" !== Type(r["[[Iterator]]"]))
      throw new Error(
        Object.prototype.toString.call(r["[[Iterator]]"]) + "is not an Object."
      );
    var e = r["[[Iterator]]"],
      o = GetMethod(e, "return");
    if (o === undefined) return t;
    try {
      var n = Call(o, e);
    } catch (c) {
      var a = c;
    }
    if (t) return t;
    if (a) throw a;
    if ("object" !== Type(n))
      throw new TypeError("Iterator's return method returned a non-object.");
    return t;
  }
  function IteratorComplete(t) {
    if ("object" !== Type(t))
      throw new Error(Object.prototype.toString.call(t) + "is not an Object.");
    return ToBoolean(Get(t, "done"));
  }
  function IteratorNext(t) {
    if (arguments.length < 2)
      var e = Call(t["[[NextMethod]]"], t["[[Iterator]]"]);
    else e = Call(t["[[NextMethod]]"], t["[[Iterator]]"], [arguments[1]]);
    if ("object" !== Type(e)) throw new TypeError("bad iterator");
    return e;
  }
  function IteratorStep(t) {
    var r = IteratorNext(t);
    return !0 !== IteratorComplete(r) && r;
  }
  function IteratorValue(t) {
    if ("object" !== Type(t))
      throw new Error(Object.prototype.toString.call(t) + "is not an Object.");
    return Get(t, "value");
  }
  function OrdinaryToPrimitive(r, t) {
    if ("string" === t) var e = ["toString", "valueOf"];
    else e = ["valueOf", "toString"];
    for (var i = 0; i < e.length; ++i) {
      var n = e[i],
        a = Get(r, n);
      if (IsCallable(a)) {
        var o = Call(a, r);
        if ("object" !== Type(o)) return o;
      }
    }
    throw new TypeError("Cannot convert to primitive.");
  }
  function SameValueZero(n, e) {
    return (
      Type(n) === Type(e) &&
      ("number" === Type(n)
        ? !(!isNaN(n) || !isNaN(e)) ||
          (1 / n === Infinity && 1 / e == -Infinity) ||
          (1 / n == -Infinity && 1 / e === Infinity) ||
          n === e
        : SameValueNonNumber(n, e))
    );
  }
  function ToInteger(n) {
    if ("symbol" === Type(n))
      throw new TypeError("Cannot convert a Symbol value to a number");
    var t = Number(n);
    return isNaN(t)
      ? 0
      : 1 / t === Infinity ||
        1 / t == -Infinity ||
        t === Infinity ||
        t === -Infinity
      ? t
      : (t < 0 ? -1 : 1) * Math.floor(Math.abs(t));
  }
  function ToLength(n) {
    var t = ToInteger(n);
    return t <= 0 ? 0 : Math.min(t, Math.pow(2, 53) - 1);
  }
  function ToPrimitive(e) {
    var t = arguments.length > 1 ? arguments[1] : undefined;
    if ("object" === Type(e)) {
      if (arguments.length < 2) var i = "default";
      else t === String ? (i = "string") : t === Number && (i = "number");
      var r =
        "function" == typeof self.Symbol &&
        "symbol" == typeof self.Symbol.toPrimitive
          ? GetMethod(e, self.Symbol.toPrimitive)
          : undefined;
      if (r !== undefined) {
        var n = Call(r, e, [i]);
        if ("object" !== Type(n)) return n;
        throw new TypeError("Cannot convert exotic object to primitive.");
      }
      return "default" === i && (i = "number"), OrdinaryToPrimitive(e, i);
    }
    return e;
  }
  function ToString(t) {
    switch (Type(t)) {
      case "symbol":
        throw new TypeError("Cannot convert a Symbol value to a string");
      case "object":
        return ToString(ToPrimitive(t, String));
      default:
        return String(t);
    }
  }
  function ToPropertyKey(r) {
    var i = ToPrimitive(r, String);
    return "symbol" === Type(i) ? i : ToString(i);
  }
  function TrimString(e, u) {
    var r = RequireObjectCoercible(e),
      t = ToString(r),
      n =
        /[\x09\x0A\x0B\x0C\x0D\x20\xA0\u1680\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200A\u202F\u205F\u3000\u2028\u2029\uFEFF]+/
          .source;
    if ("start" === u)
      var p = String.prototype.replace.call(t, new RegExp("^" + n, "g"), "");
    else
      p =
        "end" === u
          ? String.prototype.replace.call(t, new RegExp(n + "$", "g"), "")
          : String.prototype.replace.call(
              t,
              new RegExp("^" + n + "|" + n + "$", "g"),
              ""
            );
    return p;
  }
  var _mutation = (function () {
    function e(e) {
      return "function" == typeof Node
        ? e instanceof Node
        : e &&
            "object" == typeof e &&
            e.nodeName &&
            e.nodeType >= 1 &&
            e.nodeType <= 12;
    }
    return function n(t) {
      if (1 === t.length)
        return e(t[0]) ? t[0] : document.createTextNode(t[0] + "");
      for (var o = document.createDocumentFragment(), r = 0; r < t.length; r++)
        o.appendChild(e(t[r]) ? t[r] : document.createTextNode(t[r] + ""));
      return o;
    };
  })();
  CreateMethodProperty(Array, "of", function r() {
    var r = arguments.length,
      t = arguments,
      e = this;
    if (IsConstructor(e)) var a = Construct(e, [r]);
    else a = ArrayCreate(r);
    for (var o = 0; o < r; ) {
      var n = t[o],
        h = ToString(o);
      CreateDataPropertyOrThrow(a, h, n), (o += 1);
    }
    return (a.length = r), a;
  });
  CreateMethodProperty(Array.prototype, "fill", function t(e) {
    for (
      var r = arguments[1],
        n = arguments[2],
        o = ToObject(this),
        a = ToLength(Get(o, "length")),
        h = ToInteger(r),
        i = h < 0 ? Math.max(a + h, 0) : Math.min(h, a),
        g = n === undefined ? a : ToInteger(n),
        M = g < 0 ? Math.max(a + g, 0) : Math.min(g, a);
      i < M;

    ) {
      (o[ToString(i)] = e), (i += 1);
    }
    return o;
  });
  CreateMethodProperty(Array.prototype, "includes", function e(r) {
    "use strict";
    var t = ToObject(this),
      o = ToLength(Get(t, "length"));
    if (0 === o) return !1;
    var n = ToInteger(arguments[1]);
    if (n >= 0) var a = n;
    else (a = o + n) < 0 && (a = 0);
    for (; a < o; ) {
      var i = Get(t, ToString(a));
      if (SameValueZero(r, i)) return !0;
      a += 1;
    }
    return !1;
  });
  !(function (t) {
    t.DocumentFragment = function n() {
      return document.createDocumentFragment();
    };
    var e = document.createDocumentFragment();
    t.DocumentFragment.prototype = Object.create(e.constructor.prototype);
  })(self);
  !(function (t) {
    (document.createDocumentFragment().constructor.prototype.append =
      function n() {
        this.appendChild(_mutation(arguments));
      }),
      (t.DocumentFragment.prototype.append = function e() {
        this.appendChild(_mutation(arguments));
      });
  })(self);
  !(function (t) {
    (document.createDocumentFragment().constructor.prototype.prepend =
      function e() {
        this.insertBefore(_mutation(arguments), this.firstChild);
      }),
      (t.DocumentFragment.prototype.prepend = function n() {
        this.insertBefore(_mutation(arguments), this.firstChild);
      });
  })(self);
  !(function (t) {
    ("DOMTokenList" in t &&
      t.DOMTokenList &&
      (!document.createElementNS ||
        !document.createElementNS("http://www.w3.org/2000/svg", "svg") ||
        document.createElementNS("http://www.w3.org/2000/svg", "svg")
          .classList instanceof DOMTokenList)) ||
      (t.DOMTokenList = _DOMTokenList),
      (function () {
        var t = document.createElement("span");
        "classList" in t &&
          (t.classList.toggle("x", !1),
          t.classList.contains("x") &&
            (t.classList.constructor.prototype.toggle = function s(t) {
              var s = arguments[1];
              if (s === undefined) {
                var e = !this.contains(t);
                return this[e ? "add" : "remove"](t), e;
              }
              return (s = !!s), this[s ? "add" : "remove"](t), s;
            }));
      })(),
      (function () {
        var t = document.createElement("span");
        if (
          "classList" in t &&
          (t.classList.add("a", "b"), !t.classList.contains("b"))
        ) {
          var s = t.classList.constructor.prototype.add;
          t.classList.constructor.prototype.add = function () {
            for (var t = arguments, e = arguments.length, n = 0; n < e; n++)
              s.call(this, t[n]);
          };
        }
      })(),
      (function () {
        var t = document.createElement("span");
        if (
          "classList" in t &&
          (t.classList.add("a"),
          t.classList.add("b"),
          t.classList.remove("a", "b"),
          t.classList.contains("b"))
        ) {
          var s = t.classList.constructor.prototype.remove;
          t.classList.constructor.prototype.remove = function () {
            for (var t = arguments, e = arguments.length, n = 0; n < e; n++)
              s.call(this, t[n]);
          };
        }
      })();
  })(self);
  (Document.prototype.after = Element.prototype.after =
    function t() {
      if (this.parentNode) {
        for (
          var t = Array.prototype.slice.call(arguments),
            e = this.nextSibling,
            o = e ? t.indexOf(e) : -1;
          -1 !== o && (e = e.nextSibling);

        )
          o = t.indexOf(e);
        this.parentNode.insertBefore(_mutation(arguments), e);
      }
    }),
    "Text" in self && (Text.prototype.after = Element.prototype.after);
  Document.prototype.append = Element.prototype.append = function p() {
    this.appendChild(_mutation(arguments));
  };
  (Document.prototype.before = Element.prototype.before =
    function e() {
      if (this.parentNode) {
        for (
          var e = Array.prototype.slice.call(arguments),
            t = this.previousSibling,
            o = t ? e.indexOf(t) : -1;
          -1 !== o && (t = t.previousSibling);

        )
          o = e.indexOf(t);
        this.parentNode.insertBefore(
          _mutation(arguments),
          t ? t.nextSibling : this.parentNode.firstChild
        );
      }
    }),
    "Text" in self && (Text.prototype.before = Element.prototype.before);
  !(function (e) {
    var t = !0,
      r = function (e, r, n, i) {
        Object.defineProperty
          ? Object.defineProperty(e, r, {
              configurable: !1 === t || !!i,
              get: n,
            })
          : e.__defineGetter__(r, n);
      };
    try {
      r({}, "support");
    } catch (i) {
      t = !1;
    }
    var n = function (e, i, l) {
      r(
        e.prototype,
        i,
        function () {
          var e,
            c = this,
            s = "__defineGetter__DEFINE_PROPERTY" + i;
          if (c[s]) return e;
          if (((c[s] = !0), !1 === t)) {
            for (
              var o,
                a = n.mirror || document.createElement("div"),
                f = a.childNodes,
                d = f.length,
                m = 0;
              m < d;
              ++m
            )
              if (f[m]._R === c) {
                o = f[m];
                break;
              }
            o || (o = a.appendChild(document.createElement("div"))),
              (e = DOMTokenList.call(o, c, l));
          } else e = new _DOMTokenList(c, l);
          return (
            r(c, i, function () {
              return e;
            }),
            delete c[s],
            e
          );
        },
        !0
      );
    };
    n(e.Element, "classList", "className"),
      n(e.HTMLElement, "classList", "className"),
      n(e.HTMLLinkElement, "relList", "rel"),
      n(e.HTMLAnchorElement, "relList", "rel"),
      n(e.HTMLAreaElement, "relList", "rel");
  })(self);
  Element.prototype.matches =
    Element.prototype.webkitMatchesSelector ||
    Element.prototype.oMatchesSelector ||
    Element.prototype.msMatchesSelector ||
    Element.prototype.mozMatchesSelector ||
    function e(t) {
      for (
        var o = this,
          r = (o.document || o.ownerDocument).querySelectorAll(t),
          c = 0;
        r[c] && r[c] !== o;

      )
        ++c;
      return !!r[c];
    };
  Element.prototype.closest = function e(n) {
    for (var t = this; t; ) {
      if (t.matches(n)) return t;
      t =
        "SVGElement" in window && t instanceof SVGElement
          ? t.parentNode
          : t.parentElement;
    }
    return null;
  };
  Document.prototype.prepend = Element.prototype.prepend = function t() {
    this.insertBefore(_mutation(arguments), this.firstChild);
  };
  (Document.prototype.remove = Element.prototype.remove =
    function e() {
      this.parentNode && this.parentNode.removeChild(this);
    }),
    "Text" in self && (Text.prototype.remove = Element.prototype.remove);
  (Document.prototype.replaceWith = Element.prototype.replaceWith =
    function e() {
      this.parentNode &&
        this.parentNode.replaceChild(_mutation(arguments), this);
    }),
    "Text" in self &&
      (Text.prototype.replaceWith = Element.prototype.replaceWith);
  !(function () {
    function e(e, t) {
      if (!e) throw new Error("Not enough arguments");
      var n;
      if ("createEvent" in document) {
        n = document.createEvent("Event");
        var o = !(!t || t.bubbles === undefined) && t.bubbles,
          i = !(!t || t.cancelable === undefined) && t.cancelable;
        return n.initEvent(e, o, i), n;
      }
      return (
        (n = document.createEventObject()),
        (n.type = e),
        (n.bubbles = !(!t || t.bubbles === undefined) && t.bubbles),
        (n.cancelable = !(!t || t.cancelable === undefined) && t.cancelable),
        n
      );
    }
    var t = {
      click: 1,
      dblclick: 1,
      keyup: 1,
      keypress: 1,
      keydown: 1,
      mousedown: 1,
      mouseup: 1,
      mousemove: 1,
      mouseover: 1,
      mouseenter: 1,
      mouseleave: 1,
      mouseout: 1,
      storage: 1,
      storagecommit: 1,
      textinput: 1,
    };
    if ("undefined" != typeof document && "undefined" != typeof window) {
      var n = (window.Event && window.Event.prototype) || null;
      (e.NONE = 0),
        (e.CAPTURING_PHASE = 1),
        (e.AT_TARGET = 2),
        (e.BUBBLING_PHASE = 3),
        (window.Event = Window.prototype.Event = e),
        n &&
          Object.defineProperty(window.Event, "prototype", {
            configurable: !1,
            enumerable: !1,
            writable: !0,
            value: n,
          }),
        "createEvent" in document ||
          ((window.addEventListener =
            Window.prototype.addEventListener =
            Document.prototype.addEventListener =
            Element.prototype.addEventListener =
              function o() {
                var e = this,
                  n = arguments[0],
                  o = arguments[1];
                if (e === window && n in t)
                  throw new Error(
                    "In IE8 the event: " +
                      n +
                      " is not available on the window object. Please see https://github.com/Financial-Times/polyfill-service/issues/317 for more information."
                  );
                e._events || (e._events = {}),
                  e._events[n] ||
                    ((e._events[n] = function (t) {
                      var n,
                        o = e._events[t.type].list,
                        i = o.slice(),
                        r = -1,
                        c = i.length;
                      for (
                        t.preventDefault = function a() {
                          !1 !== t.cancelable && (t.returnValue = !1);
                        },
                          t.stopPropagation = function l() {
                            t.cancelBubble = !0;
                          },
                          t.stopImmediatePropagation = function s() {
                            (t.cancelBubble = !0), (t.cancelImmediate = !0);
                          },
                          t.currentTarget = e,
                          t.relatedTarget = t.fromElement || null,
                          t.target = t.target || t.srcElement || e,
                          t.timeStamp = new Date().getTime(),
                          t.clientX &&
                            ((t.pageX =
                              t.clientX + document.documentElement.scrollLeft),
                            (t.pageY =
                              t.clientY + document.documentElement.scrollTop));
                        ++r < c && !t.cancelImmediate;

                      )
                        r in i &&
                          ((n = i[r]),
                          o.includes(n) &&
                            "function" == typeof n &&
                            n.call(e, t));
                    }),
                    (e._events[n].list = []),
                    e.attachEvent && e.attachEvent("on" + n, e._events[n])),
                  e._events[n].list.push(o);
              }),
          (window.removeEventListener =
            Window.prototype.removeEventListener =
            Document.prototype.removeEventListener =
            Element.prototype.removeEventListener =
              function i() {
                var e,
                  t = this,
                  n = arguments[0],
                  o = arguments[1];
                t._events &&
                  t._events[n] &&
                  t._events[n].list &&
                  -1 !== (e = t._events[n].list.indexOf(o)) &&
                  (t._events[n].list.splice(e, 1),
                  t._events[n].list.length ||
                    (t.detachEvent && t.detachEvent("on" + n, t._events[n]),
                    delete t._events[n]));
              }),
          (window.dispatchEvent =
            Window.prototype.dispatchEvent =
            Document.prototype.dispatchEvent =
            Element.prototype.dispatchEvent =
              function r(e) {
                if (!arguments.length) throw new Error("Not enough arguments");
                if (!e || "string" != typeof e.type)
                  throw new Error("DOM Events Exception 0");
                var t = this,
                  n = e.type;
                try {
                  if (!e.bubbles) {
                    e.cancelBubble = !0;
                    var o = function (e) {
                      (e.cancelBubble = !0),
                        (t || window).detachEvent("on" + n, o);
                    };
                    this.attachEvent("on" + n, o);
                  }
                  this.fireEvent("on" + n, e);
                } catch (i) {
                  e.target = t;
                  do {
                    (e.currentTarget = t),
                      "_events" in t &&
                        "function" == typeof t._events[n] &&
                        t._events[n].call(t, e),
                      "function" == typeof t["on" + n] &&
                        t["on" + n].call(t, e),
                      (t = 9 === t.nodeType ? t.parentWindow : t.parentNode);
                  } while (t && !e.cancelBubble);
                }
                return !0;
              }),
          document.attachEvent("onreadystatechange", function () {
            "complete" === document.readyState &&
              document.dispatchEvent(
                new e("DOMContentLoaded", { bubbles: !0 })
              );
          }));
    }
  })();
  (self.CustomEvent = function e(t, n) {
    if (!t)
      throw Error(
        'TypeError: Failed to construct "CustomEvent": An event name must be provided.'
      );
    var l;
    if (
      ((n = n || { bubbles: !1, cancelable: !1, detail: null }),
      "createEvent" in document)
    )
      try {
        (l = document.createEvent("CustomEvent")),
          l.initCustomEvent(t, n.bubbles, n.cancelable, n.detail);
      } catch (a) {
        (l = document.createEvent("Event")),
          l.initEvent(t, n.bubbles, n.cancelable),
          (l.detail = n.detail);
      }
    else (l = new Event(t, n)), (l.detail = (n && n.detail) || null);
    return l;
  }),
    (CustomEvent.prototype = Event.prototype);
  !(function () {
    function e(e) {
      if (!(0 in arguments)) throw new TypeError("1 argument is required");
      do {
        if (this === e) return !0;
      } while ((e = e && e.parentNode));
      return !1;
    }
    if ("HTMLElement" in self && "contains" in HTMLElement.prototype)
      try {
        delete HTMLElement.prototype.contains;
      } catch (t) {}
    "Node" in self
      ? (Node.prototype.contains = e)
      : (document.contains = Element.prototype.contains = e);
  })();
  !(function () {
    var e = self;
    CreateMethodProperty(Number, "isNaN", function r(n) {
      return "number" === Type(n) && !!e.isNaN(n);
    });
  })();
  !(function () {
    var e = Object.getOwnPropertyDescriptor,
      t = function () {
        try {
          return (
            1 ===
            Object.defineProperty(document.createElement("div"), "one", {
              get: function () {
                return 1;
              },
            }).one
          );
        } catch (e) {
          return !1;
        }
      },
      r = {}.toString,
      n = "".split;
    CreateMethodProperty(Object, "getOwnPropertyDescriptor", function c(o, i) {
      var a = ToObject(o);
      a =
        ("string" === Type(a) || a instanceof String) &&
        "[object String]" == r.call(o)
          ? n.call(o, "")
          : Object(o);
      var u = ToPropertyKey(i);
      if (t)
        try {
          return e(a, u);
        } catch (l) {}
      if (HasOwnProperty(a, u))
        return { enumerable: !0, configurable: !0, writable: !0, value: a[u] };
    });
  })();
  !(function (e) {
    CreateMethodProperty(Object, "isExtensible", function t(n) {
      return "object" === Type(n) && (!e || e(n));
    });
  })(Object.isExtensible);
  CreateMethodProperty(
    Object,
    "keys",
    (function () {
      "use strict";
      function t() {
        var t;
        try {
          t = Object.create({});
        } catch (r) {
          return !0;
        }
        return o.call(t, "__proto__");
      }
      function r(t) {
        var r = n.call(t),
          e = "[object Arguments]" === r;
        return (
          e ||
            (e =
              "[object Array]" !== r &&
              null !== t &&
              "object" == typeof t &&
              "number" == typeof t.length &&
              t.length >= 0 &&
              "[object Function]" === n.call(t.callee)),
          e
        );
      }
      var e = Object.prototype.hasOwnProperty,
        n = Object.prototype.toString,
        o = Object.prototype.propertyIsEnumerable,
        c = !o.call({ toString: null }, "toString"),
        l = o.call(function () {}, "prototype"),
        i = [
          "toString",
          "toLocaleString",
          "valueOf",
          "hasOwnProperty",
          "isPrototypeOf",
          "propertyIsEnumerable",
          "constructor",
        ],
        u = function (t) {
          var r = t.constructor;
          return r && r.prototype === t;
        },
        a = {
          $console: !0,
          $external: !0,
          $frame: !0,
          $frameElement: !0,
          $frames: !0,
          $innerHeight: !0,
          $innerWidth: !0,
          $outerHeight: !0,
          $outerWidth: !0,
          $pageXOffset: !0,
          $pageYOffset: !0,
          $parent: !0,
          $scrollLeft: !0,
          $scrollTop: !0,
          $scrollX: !0,
          $scrollY: !0,
          $self: !0,
          $webkitIndexedDB: !0,
          $webkitStorageInfo: !0,
          $window: !0,
        },
        f = (function () {
          if ("undefined" == typeof window) return !1;
          for (var t in window)
            try {
              if (
                !a["$" + t] &&
                e.call(window, t) &&
                null !== window[t] &&
                "object" == typeof window[t]
              )
                try {
                  u(window[t]);
                } catch (r) {
                  return !0;
                }
            } catch (r) {
              return !0;
            }
          return !1;
        })(),
        p = function (t) {
          if ("undefined" == typeof window || !f) return u(t);
          try {
            return u(t);
          } catch (r) {
            return !1;
          }
        };
      return function s(o) {
        var u = "[object Function]" === n.call(o),
          a = r(o),
          f = "[object String]" === n.call(o),
          s = [];
        if (o === undefined || null === o)
          throw new TypeError("Cannot convert undefined or null to object");
        var y = l && u;
        if (f && o.length > 0 && !e.call(o, 0))
          for (var h = 0; h < o.length; ++h) s.push(String(h));
        if (a && o.length > 0)
          for (var g = 0; g < o.length; ++g) s.push(String(g));
        else
          for (var w in o)
            (t() && "__proto__" === w) ||
              (y && "prototype" === w) ||
              !e.call(o, w) ||
              s.push(String(w));
        if (c)
          for (var d = p(o), $ = 0; $ < i.length; ++$)
            (d && "constructor" === i[$]) || !e.call(o, i[$]) || s.push(i[$]);
        return s;
      };
    })()
  );
  CreateMethodProperty(Object, "assign", function e(t, r) {
    var n = ToObject(t);
    if (1 === arguments.length) return n;
    var o,
      c,
      a,
      l,
      i = Array.prototype.slice.call(arguments, 1);
    for (o = 0; o < i.length; o++) {
      var p = i[o];
      for (
        p === undefined || null === p
          ? (a = [])
          : ((l =
              "[object String]" === Object.prototype.toString.call(p)
                ? String(p).split("")
                : ToObject(p)),
            (a = Object.keys(l))),
          c = 0;
        c < a.length;
        c++
      ) {
        var b,
          y = a[c];
        try {
          var g = Object.getOwnPropertyDescriptor(l, y);
          b = g !== undefined && !0 === g.enumerable;
        } catch (u) {
          b = Object.prototype.propertyIsEnumerable.call(l, y);
        }
        if (b) {
          var j = Get(l, y);
          n[y] = j;
        }
      }
    }
    return n;
  });
  !(function () {
    var t = {}.toString,
      e = "".split,
      r = [].concat,
      o = Object.prototype.hasOwnProperty,
      c = Object.getOwnPropertyNames || Object.keys,
      n = "object" == typeof self ? c(self) : [];
    CreateMethodProperty(Object, "getOwnPropertyNames", function l(a) {
      var p = ToObject(a);
      if ("[object Window]" === t.call(p))
        try {
          return c(p);
        } catch (j) {
          return r.call([], n);
        }
      p = "[object String]" == t.call(p) ? e.call(p, "") : Object(p);
      for (
        var i = c(p), s = ["length", "prototype"], O = 0;
        O < s.length;
        O++
      ) {
        var b = s[O];
        o.call(p, b) && !i.includes(b) && i.push(b);
      }
      if (i.includes("__proto__")) {
        var f = i.indexOf("__proto__");
        i.splice(f, 1);
      }
      return i;
    });
  })();
  CreateMethodProperty(String.prototype, "endsWith", function e(t) {
    "use strict";
    var r = arguments.length > 1 ? arguments[1] : undefined,
      n = RequireObjectCoercible(this),
      i = ToString(n);
    if (IsRegExp(t))
      throw new TypeError(
        "First argument to String.prototype.endsWith must not be a regular expression"
      );
    var o = ToString(t),
      s = i.length,
      g = r === undefined ? s : ToInteger(r),
      h = Math.min(Math.max(g, 0), s),
      u = o.length,
      a = h - u;
    return !(a < 0) && i.substr(a, u) === o;
  });
  CreateMethodProperty(String.prototype, "includes", function e(t) {
    "use strict";
    var r = arguments.length > 1 ? arguments[1] : undefined,
      n = RequireObjectCoercible(this),
      i = ToString(n);
    if (IsRegExp(t))
      throw new TypeError(
        "First argument to String.prototype.includes must not be a regular expression"
      );
    var o = ToString(t),
      g = ToInteger(r),
      a = i.length,
      p = Math.min(Math.max(g, 0), a);
    return -1 !== String.prototype.indexOf.call(i, o, p);
  });
  CreateMethodProperty(String.prototype, "startsWith", function t(e) {
    "use strict";
    var r = arguments.length > 1 ? arguments[1] : undefined,
      n = RequireObjectCoercible(this),
      i = ToString(n);
    if (IsRegExp(e))
      throw new TypeError(
        "First argument to String.prototype.startsWith must not be a regular expression"
      );
    var o = ToString(e),
      s = ToInteger(r),
      a = i.length,
      g = Math.min(Math.max(s, 0), a);
    return !(o.length + g > a) && 0 === i.substr(g).indexOf(e);
  });
  CreateMethodProperty(String.prototype, "trim", function t() {
    "use strict";
    var t = this;
    return TrimString(t, "start+end");
  });
  !(function (e, r, n) {
    "use strict";
    function t(e) {
      if ("symbol" === Type(e)) return e;
      throw TypeError(e + " is not a symbol");
    }
    var u,
      o = (function () {
        try {
          var r = {};
          return (
            e.defineProperty(r, "t", {
              configurable: !0,
              enumerable: !1,
              get: function () {
                return !0;
              },
              set: undefined,
            }),
            !!r.t
          );
        } catch (n) {
          return !1;
        }
      })(),
      i = 0,
      a = "" + Math.random(),
      c = "__symbol:",
      l = c.length,
      f = "__symbol@@" + a,
      s = {},
      v = "defineProperty",
      y = "defineProperties",
      b = "getOwnPropertyNames",
      p = "getOwnPropertyDescriptor",
      h = "propertyIsEnumerable",
      m = e.prototype,
      d = m.hasOwnProperty,
      g = m[h],
      w = m.toString,
      S = Array.prototype.concat,
      P = e.getOwnPropertyNames ? e.getOwnPropertyNames(self) : [],
      O = e[b],
      j = function $(e) {
        if ("[object Window]" === w.call(e))
          try {
            return O(e);
          } catch (r) {
            return S.call([], P);
          }
        return O(e);
      },
      E = e[p],
      N = e.create,
      T = e.keys,
      _ = e.freeze || e,
      k = e[v],
      F = e[y],
      I = E(e, b),
      x = function (e, r, n) {
        if (!d.call(e, f))
          try {
            k(e, f, {
              enumerable: !1,
              configurable: !1,
              writable: !1,
              value: {},
            });
          } catch (t) {
            e[f] = {};
          }
        e[f]["@@" + r] = n;
      },
      z = function (e, r) {
        var n = N(e);
        return (
          j(r).forEach(function (e) {
            q.call(r, e) && L(n, e, r[e]);
          }),
          n
        );
      },
      A = function (e) {
        var r = N(e);
        return (r.enumerable = !1), r;
      },
      D = function ee() {},
      M = function (e) {
        return e != f && !d.call(H, e);
      },
      W = function (e) {
        return e != f && d.call(H, e);
      },
      q = function re(e) {
        var r = "" + e;
        return W(r)
          ? d.call(this, r) && this[f] && this[f]["@@" + r]
          : g.call(this, e);
      },
      B = function (r) {
        var n = {
          enumerable: !1,
          configurable: !0,
          get: D,
          set: function (e) {
            u(this, r, {
              enumerable: !1,
              configurable: !0,
              writable: !0,
              value: e,
            }),
              x(this, r, !0);
          },
        };
        try {
          k(m, r, n);
        } catch (o) {
          m[r] = n.value;
        }
        H[r] = k(e(r), "constructor", J);
        var t = E(G.prototype, "description");
        return t && k(H[r], "description", t), _(H[r]);
      },
      C = function (e) {
        var r = t(e);
        if (Y) {
          var n = V(r);
          if ("" !== n) return n.slice(1, -1);
        }
        if (s[r] !== undefined) return s[r];
        var u = r.toString(),
          o = u.lastIndexOf("0.");
        return (u = u.slice(10, o)), "" === u ? undefined : u;
      },
      G = function ne() {
        var r = arguments[0];
        if (this instanceof ne)
          throw new TypeError("Symbol is not a constructor");
        var n = c.concat(r || "", a, ++i);
        r === undefined ||
          (null !== r && !isNaN(r) && "" !== String(r)) ||
          (s[n] = String(r));
        var t = B(n);
        return (
          o ||
            e.defineProperty(t, "description", {
              configurable: !0,
              enumerable: !1,
              value: C(t),
            }),
          t
        );
      },
      H = N(null),
      J = { value: G },
      K = function (e) {
        return H[e];
      },
      L = function te(e, r, n) {
        var t = "" + r;
        return (
          W(t)
            ? (u(e, t, n.enumerable ? A(n) : n), x(e, t, !!n.enumerable))
            : k(e, r, n),
          e
        );
      },
      Q = function (e) {
        return function (r) {
          return d.call(e, f) && d.call(e[f], "@@" + r);
        };
      },
      R = function ue(e) {
        return j(e)
          .filter(e === m ? Q(e) : W)
          .map(K);
      };
    (I.value = L),
      k(e, v, I),
      (I.value = R),
      k(e, "getOwnPropertySymbols", I),
      (I.value = function oe(e) {
        return j(e).filter(M);
      }),
      k(e, b, I),
      (I.value = function ie(e, r) {
        var n = R(r);
        return (
          n.length
            ? T(r)
                .concat(n)
                .forEach(function (n) {
                  q.call(r, n) && L(e, n, r[n]);
                })
            : F(e, r),
          e
        );
      }),
      k(e, y, I),
      (I.value = q),
      k(m, h, I),
      (I.value = G),
      k(n, "Symbol", I),
      (I.value = function (e) {
        var r = c.concat(c, e, a);
        return r in m ? H[r] : B(r);
      }),
      k(G, "for", I),
      (I.value = function (e) {
        if (M(e)) throw new TypeError(e + " is not a symbol");
        return d.call(H, e) ? e.slice(2 * l, -a.length) : void 0;
      }),
      k(G, "keyFor", I),
      (I.value = function ae(e, r) {
        var n = E(e, r);
        return n && W(r) && (n.enumerable = q.call(e, r)), n;
      }),
      k(e, p, I),
      (I.value = function ce(e, r) {
        return 1 === arguments.length || void 0 === r ? N(e) : z(e, r);
      }),
      k(e, "create", I);
    var U =
      null ===
      function () {
        return this;
      }.call(null);
    if (
      ((I.value = U
        ? function () {
            var e = w.call(this);
            return "[object String]" === e && W(this) ? "[object Symbol]" : e;
          }
        : function () {
            if (this === window) return "[object Null]";
            var e = w.call(this);
            return "[object String]" === e && W(this) ? "[object Symbol]" : e;
          }),
      k(m, "toString", I),
      (u = function (e, r, n) {
        var t = E(m, r);
        delete m[r], k(e, r, n), e !== m && k(m, r, t);
      }),
      (function () {
        try {
          var r = {};
          return (
            e.defineProperty(r, "t", {
              configurable: !0,
              enumerable: !1,
              get: function () {
                return !0;
              },
              set: undefined,
            }),
            !!r.t
          );
        } catch (n) {
          return !1;
        }
      })())
    ) {
      var V;
      try {
        V = Function("s", "var v = s.valueOf(); return { [v]() {} }[v].name;");
      } catch (Z) {}
      var X = function () {},
        Y = V && "inferred" === X.name ? V : null;
      e.defineProperty(n.Symbol.prototype, "description", {
        configurable: !0,
        enumerable: !1,
        get: function () {
          return C(this);
        },
      });
    }
  })(Object, 0, self);
  Object.defineProperty(self.Symbol, "iterator", {
    value: self.Symbol("iterator"),
  });
  function GetIterator(t) {
    var e = arguments.length > 1 ? arguments[1] : GetMethod(t, Symbol.iterator),
      r = Call(e, t);
    if ("object" !== Type(r)) throw new TypeError("bad iterator");
    var o = GetV(r, "next"),
      a = Object.create(null);
    return (
      (a["[[Iterator]]"] = r),
      (a["[[NextMethod]]"] = o),
      (a["[[Done]]"] = !1),
      a
    );
  }
  Object.defineProperty(Symbol, "species", { value: Symbol("species") });
  !(function (e) {
    function t(e, t) {
      if ("object" !== Type(e))
        throw new TypeError(
          "createMapIterator called on incompatible receiver " +
            Object.prototype.toString.call(e)
        );
      if (!0 !== e._es6Map)
        throw new TypeError(
          "createMapIterator called on incompatible receiver " +
            Object.prototype.toString.call(e)
        );
      var r = Object.create(u);
      return (
        Object.defineProperty(r, "[[Map]]", {
          configurable: !0,
          enumerable: !1,
          writable: !0,
          value: e,
        }),
        Object.defineProperty(r, "[[MapNextIndex]]", {
          configurable: !0,
          enumerable: !1,
          writable: !0,
          value: 0,
        }),
        Object.defineProperty(r, "[[MapIterationKind]]", {
          configurable: !0,
          enumerable: !1,
          writable: !0,
          value: t,
        }),
        r
      );
    }
    var r = (function () {
        try {
          var e = {};
          return (
            Object.defineProperty(e, "t", {
              configurable: !0,
              enumerable: !1,
              get: function () {
                return !0;
              },
              set: undefined,
            }),
            !!e.t
          );
        } catch (t) {
          return !1;
        }
      })(),
      o = 0,
      a = Symbol("meta_" + (1e8 * Math.random() + "").replace(".", "")),
      n = function (e) {
        if ("object" == typeof e ? null !== e : "function" == typeof e) {
          if (!Object.isExtensible(e)) return !1;
          if (!Object.prototype.hasOwnProperty.call(e, a)) {
            var t = typeof e + "-" + ++o;
            Object.defineProperty(e, a, {
              configurable: !1,
              enumerable: !1,
              writable: !1,
              value: t,
            });
          }
          return e[a];
        }
        return "" + e;
      },
      i = function (e, t) {
        var r = n(t);
        if (!1 === r) return p(e, t);
        var o = e._table[r];
        return o !== undefined && o;
      },
      p = function (e, t) {
        for (var r = 0; r < e._keys.length; r++) {
          var o = e._keys[r];
          if (o !== c && SameValueZero(o, t)) return r;
        }
        return !1;
      },
      l = function (e, t, r) {
        var o = n(t);
        return (
          !1 !== o && (!1 === r ? delete e._table[o] : (e._table[o] = r), !0)
        );
      },
      c = Symbol("undef"),
      y = function f() {
        if (!(this instanceof f))
          throw new TypeError('Constructor Map requires "new"');
        var e = OrdinaryCreateFromConstructor(this, f.prototype, {
          _table: {},
          _keys: [],
          _values: [],
          _size: 0,
          _es6Map: !0,
        });
        r ||
          Object.defineProperty(e, "size", {
            configurable: !0,
            enumerable: !1,
            writable: !0,
            value: 0,
          });
        var t = arguments.length > 0 ? arguments[0] : undefined;
        if (null === t || t === undefined) return e;
        var o = e.set;
        if (!IsCallable(o))
          throw new TypeError("Map.prototype.set is not a function");
        try {
          for (var a = GetIterator(t); ; ) {
            var n = IteratorStep(a);
            if (!1 === n) return e;
            var i = IteratorValue(n);
            if ("object" !== Type(i))
              try {
                throw new TypeError(
                  "Iterator value " + i + " is not an entry object"
                );
              } catch (u) {
                return IteratorClose(a, u);
              }
            try {
              var p = i[0],
                l = i[1];
              o.call(e, p, l);
            } catch (s) {
              return IteratorClose(a, s);
            }
          }
        } catch (s) {
          if (
            Array.isArray(t) ||
            "[object Arguments]" === Object.prototype.toString.call(t) ||
            t.callee
          ) {
            var c,
              y = t.length;
            for (c = 0; c < y; c++) o.call(e, t[c][0], t[c][1]);
          }
        }
        return e;
      };
    Object.defineProperty(y, "prototype", {
      configurable: !1,
      enumerable: !1,
      writable: !1,
      value: {},
    }),
      r
        ? Object.defineProperty(y, Symbol.species, {
            configurable: !0,
            enumerable: !1,
            get: function () {
              return this;
            },
            set: undefined,
          })
        : CreateMethodProperty(y, Symbol.species, y),
      CreateMethodProperty(y.prototype, "clear", function b() {
        var e = this;
        if ("object" !== Type(e))
          throw new TypeError(
            "Method Map.prototype.clear called on incompatible receiver " +
              Object.prototype.toString.call(e)
          );
        if (!0 !== e._es6Map)
          throw new TypeError(
            "Method Map.prototype.clear called on incompatible receiver " +
              Object.prototype.toString.call(e)
          );
        for (var t = e._keys, o = 0; o < t.length; o++)
          (e._keys[o] = c), (e._values[o] = c);
        return (
          (this._size = 0),
          r || (this.size = this._size),
          (this._table = {}),
          undefined
        );
      }),
      CreateMethodProperty(y.prototype, "constructor", y),
      CreateMethodProperty(y.prototype, "delete", function (e) {
        var t = this;
        if ("object" !== Type(t))
          throw new TypeError(
            "Method Map.prototype.clear called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        if (!0 !== t._es6Map)
          throw new TypeError(
            "Method Map.prototype.clear called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        var o = i(t, e);
        if (!1 !== o) {
          var a = t._keys[o];
          if (a !== c && SameValueZero(a, e))
            return (
              (this._keys[o] = c),
              (this._values[o] = c),
              (this._size = --this._size),
              r || (this.size = this._size),
              l(this, e, !1),
              !0
            );
        }
        return !1;
      }),
      CreateMethodProperty(y.prototype, "entries", function h() {
        return t(this, "key+value");
      }),
      CreateMethodProperty(y.prototype, "forEach", function (e) {
        var t = this;
        if ("object" !== Type(t))
          throw new TypeError(
            "Method Map.prototype.forEach called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        if (!0 !== t._es6Map)
          throw new TypeError(
            "Method Map.prototype.forEach called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        if (!IsCallable(e))
          throw new TypeError(
            Object.prototype.toString.call(e) + " is not a function."
          );
        if (arguments[1]) var r = arguments[1];
        for (var o = t._keys, a = 0; a < o.length; a++)
          t._keys[a] !== c &&
            t._values[a] !== c &&
            e.call(r, t._values[a], t._keys[a], t);
        return undefined;
      }),
      CreateMethodProperty(y.prototype, "get", function d(e) {
        var t = this;
        if ("object" !== Type(t))
          throw new TypeError(
            "Method Map.prototype.get called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        if (!0 !== t._es6Map)
          throw new TypeError(
            "Method Map.prototype.get called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        var r = i(t, e);
        if (!1 !== r) {
          var o = t._keys[r];
          if (o !== c && SameValueZero(o, e)) return t._values[r];
        }
        return undefined;
      }),
      CreateMethodProperty(y.prototype, "has", function v(e) {
        var t = this;
        if ("object" != typeof t)
          throw new TypeError(
            "Method Map.prototype.has called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        if (!0 !== t._es6Map)
          throw new TypeError(
            "Method Map.prototype.has called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        var r = i(t, e);
        if (!1 !== r) {
          var o = t._keys[r];
          if (o !== c && SameValueZero(o, e)) return !0;
        }
        return !1;
      }),
      CreateMethodProperty(y.prototype, "keys", function M() {
        return t(this, "key");
      }),
      CreateMethodProperty(y.prototype, "set", function w(e, t) {
        var o = this;
        if ("object" !== Type(o))
          throw new TypeError(
            "Method Map.prototype.set called on incompatible receiver " +
              Object.prototype.toString.call(o)
          );
        if (!0 !== o._es6Map)
          throw new TypeError(
            "Method Map.prototype.set called on incompatible receiver " +
              Object.prototype.toString.call(o)
          );
        var a = i(o, e);
        if (!1 !== a) o._values[a] = t;
        else {
          -0 === e && (e = 0);
          var n = { "[[Key]]": e, "[[Value]]": t };
          o._keys.push(n["[[Key]]"]),
            o._values.push(n["[[Value]]"]),
            l(o, e, o._keys.length - 1),
            ++o._size,
            r || (o.size = o._size);
        }
        return o;
      }),
      r &&
        Object.defineProperty(y.prototype, "size", {
          configurable: !0,
          enumerable: !1,
          get: function () {
            var e = this;
            if ("object" !== Type(e))
              throw new TypeError(
                "Method Map.prototype.size called on incompatible receiver " +
                  Object.prototype.toString.call(e)
              );
            if (!0 !== e._es6Map)
              throw new TypeError(
                "Method Map.prototype.size called on incompatible receiver " +
                  Object.prototype.toString.call(e)
              );
            return this._size;
          },
          set: undefined,
        }),
      CreateMethodProperty(y.prototype, "values", function j() {
        return t(this, "value");
      }),
      CreateMethodProperty(y.prototype, Symbol.iterator, y.prototype.entries),
      "name" in y ||
        Object.defineProperty(y, "name", {
          configurable: !0,
          enumerable: !1,
          writable: !1,
          value: "Map",
        });
    var u = {};
    Object.defineProperty(u, "isMapIterator", {
      configurable: !1,
      enumerable: !1,
      writable: !1,
      value: !0,
    }),
      CreateMethodProperty(u, "next", function _() {
        var e = this;
        if ("object" !== Type(e))
          throw new TypeError(
            "Method %MapIteratorPrototype%.next called on incompatible receiver " +
              Object.prototype.toString.call(e)
          );
        if (!e.isMapIterator)
          throw new TypeError(
            "Method %MapIteratorPrototype%.next called on incompatible receiver " +
              Object.prototype.toString.call(e)
          );
        var t = e["[[Map]]"],
          r = e["[[MapNextIndex]]"],
          o = e["[[MapIterationKind]]"];
        if (t === undefined) return CreateIterResultObject(undefined, !0);
        if (!t._es6Map)
          throw new Error(
            Object.prototype.toString.call(t) +
              " has a [[MapData]] internal slot."
          );
        for (var a = t._keys, n = a.length; r < n; ) {
          var i = Object.create(null);
          if (
            ((i["[[Key]]"] = t._keys[r]),
            (i["[[Value]]"] = t._values[r]),
            (r += 1),
            (e["[[MapNextIndex]]"] = r),
            i["[[Key]]"] !== c)
          ) {
            if ("key" === o) var p = i["[[Key]]"];
            else if ("value" === o) p = i["[[Value]]"];
            else {
              if ("key+value" !== o) throw new Error();
              p = [i["[[Key]]"], i["[[Value]]"]];
            }
            return CreateIterResultObject(p, !1);
          }
        }
        return (
          (e["[[Map]]"] = undefined), CreateIterResultObject(undefined, !0)
        );
      }),
      CreateMethodProperty(u, Symbol.iterator, function g() {
        return this;
      });
    try {
      CreateMethodProperty(e, "Map", y);
    } catch (s) {
      e.Map = y;
    }
  })(self);
  !(function (e) {
    function t(e, t) {
      if ("object" != typeof e)
        throw new TypeError(
          "createSetIterator called on incompatible receiver " +
            Object.prototype.toString.call(e)
        );
      if (!0 !== e._es6Set)
        throw new TypeError(
          "createSetIterator called on incompatible receiver " +
            Object.prototype.toString.call(e)
        );
      var r = Object.create(i);
      return (
        Object.defineProperty(r, "[[IteratedSet]]", {
          configurable: !0,
          enumerable: !1,
          writable: !0,
          value: e,
        }),
        Object.defineProperty(r, "[[SetNextIndex]]", {
          configurable: !0,
          enumerable: !1,
          writable: !0,
          value: 0,
        }),
        Object.defineProperty(r, "[[SetIterationKind]]", {
          configurable: !0,
          enumerable: !1,
          writable: !0,
          value: t,
        }),
        r
      );
    }
    var r = (function () {
        try {
          var e = {};
          return (
            Object.defineProperty(e, "t", {
              configurable: !0,
              enumerable: !1,
              get: function () {
                return !0;
              },
              set: undefined,
            }),
            !!e.t
          );
        } catch (t) {
          return !1;
        }
      })(),
      o = Symbol("undef"),
      n = function c() {
        if (!(this instanceof c))
          throw new TypeError('Constructor Set requires "new"');
        var e = OrdinaryCreateFromConstructor(this, c.prototype, {
          _values: [],
          _size: 0,
          _es6Set: !0,
        });
        r ||
          Object.defineProperty(e, "size", {
            configurable: !0,
            enumerable: !1,
            writable: !0,
            value: 0,
          });
        var t = arguments.length > 0 ? arguments[0] : undefined;
        if (null === t || t === undefined) return e;
        var o = e.add;
        if (!IsCallable(o))
          throw new TypeError("Set.prototype.add is not a function");
        try {
          for (var n = GetIterator(t); ; ) {
            var a = IteratorStep(n);
            if (!1 === a) return e;
            var i = IteratorValue(a);
            try {
              o.call(e, i);
            } catch (y) {
              return IteratorClose(n, y);
            }
          }
        } catch (y) {
          if (
            !Array.isArray(t) &&
            "[object Arguments]" !== Object.prototype.toString.call(t) &&
            !t.callee
          )
            throw y;
          var l,
            p = t.length;
          for (l = 0; l < p; l++) o.call(e, t[l]);
        }
        return e;
      };
    Object.defineProperty(n, "prototype", {
      configurable: !1,
      enumerable: !1,
      writable: !1,
      value: {},
    }),
      r
        ? Object.defineProperty(n, Symbol.species, {
            configurable: !0,
            enumerable: !1,
            get: function () {
              return this;
            },
            set: undefined,
          })
        : CreateMethodProperty(n, Symbol.species, n),
      CreateMethodProperty(n.prototype, "add", function p(e) {
        var t = this;
        if ("object" != typeof t)
          throw new TypeError(
            "Method Set.prototype.add called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        if (!0 !== t._es6Set)
          throw new TypeError(
            "Method Set.prototype.add called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        for (var n = t._values, a = 0; a < n.length; a++) {
          var i = n[a];
          if (i !== o && SameValueZero(i, e)) return t;
        }
        return (
          0 === e && 1 / e == -Infinity && (e = 0),
          t._values.push(e),
          (this._size = ++this._size),
          r || (this.size = this._size),
          t
        );
      }),
      CreateMethodProperty(n.prototype, "clear", function y() {
        var e = this;
        if ("object" != typeof e)
          throw new TypeError(
            "Method Set.prototype.clear called on incompatible receiver " +
              Object.prototype.toString.call(e)
          );
        if (!0 !== e._es6Set)
          throw new TypeError(
            "Method Set.prototype.clear called on incompatible receiver " +
              Object.prototype.toString.call(e)
          );
        for (var t = e._values, n = 0; n < t.length; n++) t[n] = o;
        return (this._size = 0), r || (this.size = this._size), undefined;
      }),
      CreateMethodProperty(n.prototype, "constructor", n),
      CreateMethodProperty(n.prototype, "delete", function (e) {
        var t = this;
        if ("object" != typeof t)
          throw new TypeError(
            "Method Set.prototype.delete called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        if (!0 !== t._es6Set)
          throw new TypeError(
            "Method Set.prototype.delete called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        for (var n = t._values, a = 0; a < n.length; a++) {
          var i = n[a];
          if (i !== o && SameValueZero(i, e))
            return (
              (n[a] = o),
              (this._size = --this._size),
              r || (this.size = this._size),
              !0
            );
        }
        return !1;
      }),
      CreateMethodProperty(n.prototype, "entries", function u() {
        return t(this, "key+value");
      }),
      CreateMethodProperty(n.prototype, "forEach", function f(e) {
        var t = this;
        if ("object" != typeof t)
          throw new TypeError(
            "Method Set.prototype.forEach called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        if (!0 !== t._es6Set)
          throw new TypeError(
            "Method Set.prototype.forEach called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        if (!IsCallable(e))
          throw new TypeError(
            Object.prototype.toString.call(e) + " is not a function."
          );
        if (arguments[1]) var r = arguments[1];
        for (var n = t._values, a = 0; a < n.length; a++) {
          var i = n[a];
          i !== o && e.call(r, i, i, t);
        }
        return undefined;
      }),
      CreateMethodProperty(n.prototype, "has", function d(e) {
        var t = this;
        if ("object" != typeof t)
          throw new TypeError(
            "Method Set.prototype.forEach called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        if (!0 !== t._es6Set)
          throw new TypeError(
            "Method Set.prototype.forEach called on incompatible receiver " +
              Object.prototype.toString.call(t)
          );
        for (var r = t._values, n = 0; n < r.length; n++) {
          var a = r[n];
          if (a !== o && SameValueZero(a, e)) return !0;
        }
        return !1;
      });
    var a = function h() {
      return t(this, "value");
    };
    CreateMethodProperty(n.prototype, "values", a),
      CreateMethodProperty(n.prototype, "keys", a),
      r &&
        Object.defineProperty(n.prototype, "size", {
          configurable: !0,
          enumerable: !1,
          get: function () {
            var e = this;
            if ("object" != typeof e)
              throw new TypeError(
                "Method Set.prototype.size called on incompatible receiver " +
                  Object.prototype.toString.call(e)
              );
            if (!0 !== e._es6Set)
              throw new TypeError(
                "Method Set.prototype.size called on incompatible receiver " +
                  Object.prototype.toString.call(e)
              );
            for (var t = e._values, r = 0, n = 0; n < t.length; n++) {
              t[n] !== o && (r += 1);
            }
            return r;
          },
          set: undefined,
        }),
      CreateMethodProperty(n.prototype, Symbol.iterator, a),
      "name" in n ||
        Object.defineProperty(n, "name", {
          configurable: !0,
          enumerable: !1,
          writable: !1,
          value: "Set",
        });
    var i = {};
    Object.defineProperty(i, "isSetIterator", {
      configurable: !1,
      enumerable: !1,
      writable: !1,
      value: !0,
    }),
      CreateMethodProperty(i, "next", function b() {
        var e = this;
        if ("object" != typeof e)
          throw new TypeError(
            "Method %SetIteratorPrototype%.next called on incompatible receiver " +
              Object.prototype.toString.call(e)
          );
        if (!e.isSetIterator)
          throw new TypeError(
            "Method %SetIteratorPrototype%.next called on incompatible receiver " +
              Object.prototype.toString.call(e)
          );
        var t = e["[[IteratedSet]]"],
          r = e["[[SetNextIndex]]"],
          n = e["[[SetIterationKind]]"];
        if (t === undefined) return CreateIterResultObject(undefined, !0);
        if (!t._es6Set)
          throw new Error(
            Object.prototype.toString.call(t) +
              " does not have [[SetData]] internal slot."
          );
        for (var a = t._values, i = a.length; r < i; ) {
          var l = a[r];
          if (((r += 1), (e["[[SetNextIndex]]"] = r), l !== o))
            return "key+value" === n
              ? CreateIterResultObject([l, l], !1)
              : CreateIterResultObject(l, !1);
        }
        return (
          (e["[[IteratedSet]]"] = undefined),
          CreateIterResultObject(undefined, !0)
        );
      }),
      CreateMethodProperty(i, Symbol.iterator, function s() {
        return this;
      });
    try {
      CreateMethodProperty(e, "Set", n);
    } catch (l) {
      e.Set = n;
    }
  })(self);
  !(function () {
    function r(r) {
      return (
        "string" == typeof r ||
        ("object" == typeof r && "[object String]" === t.call(r))
      );
    }
    var t = Object.prototype.toString,
      e = String.prototype.match;
    CreateMethodProperty(Array, "from", function o(t) {
      var o = this,
        a = arguments.length > 1 ? arguments[1] : undefined;
      if (a === undefined) var n = !1;
      else {
        if (!1 === IsCallable(a))
          throw new TypeError(
            Object.prototype.toString.call(a) + " is not a function."
          );
        var i = arguments.length > 2 ? arguments[2] : undefined;
        if (i !== undefined) var l = i;
        else l = undefined;
        n = !0;
      }
      var u = GetMethod(t, Symbol.iterator);
      if (u !== undefined) {
        if (IsConstructor(o)) var f = Construct(o);
        else f = ArrayCreate(0);
        for (var c = GetIterator(t, u), s = 0; ; ) {
          if (s >= Math.pow(2, 53) - 1) {
            var h = new TypeError(
              "Iteration count can not be greater than or equal 9007199254740991."
            );
            return IteratorClose(c, h);
          }
          var y = ToString(s),
            C = IteratorStep(c);
          if (!1 === C) return (f.length = s), f;
          var g = IteratorValue(C);
          if (n)
            try {
              var p = Call(a, l, [g, s]);
            } catch (b) {
              return IteratorClose(c, b);
            }
          else p = g;
          try {
            CreateDataPropertyOrThrow(f, y, p);
          } catch (b) {
            return IteratorClose(c, b);
          }
          s += 1;
        }
      }
      if (r(t))
        var v =
          e.call(t, /[\uD800-\uDBFF][\uDC00-\uDFFF]?|[^\uD800-\uDFFF]|./g) ||
          [];
      else v = ToObject(t);
      var d = ToLength(Get(v, "length"));
      for (
        f = IsConstructor(o) ? Construct(o, [d]) : ArrayCreate(d), s = 0;
        s < d;

      ) {
        y = ToString(s);
        var I = Get(v, y);
        (p = !0 === n ? Call(a, l, [I, s]) : I),
          CreateDataPropertyOrThrow(f, y, p),
          (s += 1);
      }
      return (f.length = d), f;
    });
  })();
  Object.defineProperty(Symbol, "toStringTag", {
    value: Symbol("toStringTag"),
  });
  !(function () {
    "use strict";
    function n() {
      return tn[q][B] || D;
    }
    function t(n) {
      return n && "object" == typeof n;
    }
    function e(n) {
      return "function" == typeof n;
    }
    function r(n, t) {
      return n instanceof t;
    }
    function o(n) {
      return r(n, A);
    }
    function i(n, t, e) {
      if (!t(n)) throw a(e);
    }
    function u() {
      try {
        return b.apply(R, arguments);
      } catch (n) {
        return (Y.e = n), Y;
      }
    }
    function c(n, t) {
      return (b = n), (R = t), u;
    }
    function f(n, t) {
      function e() {
        for (var e = 0; e < o; ) t(r[e], r[e + 1]), (r[e++] = T), (r[e++] = T);
        (o = 0), r.length > n && (r.length = n);
      }
      var r = L(n),
        o = 0;
      return function (n, t) {
        (r[o++] = n), (r[o++] = t), 2 === o && tn.nextTick(e);
      };
    }
    function s(n, t) {
      var o,
        i,
        u,
        f,
        s = 0;
      if (!n) throw a(N);
      var l = n[tn[q][z]];
      if (e(l)) i = l.call(n);
      else {
        if (!e(n.next)) {
          if (r(n, L)) {
            for (o = n.length; s < o; ) t(n[s], s++);
            return s;
          }
          throw a(N);
        }
        i = n;
      }
      for (; !(u = i.next()).done; )
        if ((f = c(t)(u.value, s++)) === Y) throw (e(i[G]) && i[G](), f.e);
      return s;
    }
    function a(n) {
      return new TypeError(n);
    }
    function l(n) {
      return (n ? "" : Q) + new A().stack;
    }
    function h(n, t) {
      var e = "on" + n.toLowerCase(),
        r = F[e];
      E && E.listeners(n).length
        ? n === X
          ? E.emit(n, t._v, t)
          : E.emit(n, t)
        : r
        ? r({ reason: t._v, promise: t })
        : tn[n](t._v, t);
    }
    function v(n) {
      return n && n._s;
    }
    function _(n) {
      if (v(n)) return new n(Z);
      var t, r, o;
      return (
        (t = new n(function (n, e) {
          if (t) throw a();
          (r = n), (o = e);
        })),
        i(r, e),
        i(o, e),
        t
      );
    }
    function d(n, t) {
      var e = !1;
      return function (r) {
        e || ((e = !0), I && (n[M] = l(!0)), t === U ? g(n, r) : y(n, t, r));
      };
    }
    function p(n, t, r, o) {
      return (
        e(r) && (t._onFulfilled = r),
        e(o) && (n[J] && h(W, n), (t._onRejected = o)),
        I && (t._p = n),
        (n[n._c++] = t),
        n._s !== $ && rn(n, t),
        t
      );
    }
    function m(n) {
      if (n._umark) return !0;
      n._umark = !0;
      for (var t, e = 0, r = n._c; e < r; )
        if (((t = n[e++]), t._onRejected || m(t))) return !0;
    }
    function w(n, t) {
      function e(n) {
        return r.push(n.replace(/^\s+|\s+$/g, ""));
      }
      var r = [];
      return (
        I &&
          (t[M] && e(t[M]),
          (function o(n) {
            n && K in n && (o(n._next), e(n[K] + ""), o(n._p));
          })(t)),
        (n && n.stack ? n.stack : n) + ("\n" + r.join("\n")).replace(nn, "")
      );
    }
    function j(n, t) {
      return n(t);
    }
    function y(n, t, e) {
      var r = 0,
        i = n._c;
      if (n._s === $)
        for (
          n._s = t,
            n._v = e,
            t === O && (I && o(e) && (e.longStack = w(e, n)), on(n));
          r < i;

        )
          rn(n, n[r++]);
      return n;
    }
    function g(n, r) {
      if (r === n && r) return y(n, O, a(V)), n;
      if (r !== S && (e(r) || t(r))) {
        var o = c(k)(r);
        if (o === Y) return y(n, O, o.e), n;
        e(o)
          ? (I && v(r) && (n._next = r),
            v(r)
              ? x(n, r, o)
              : tn.nextTick(function () {
                  x(n, r, o);
                }))
          : y(n, U, r);
      } else y(n, U, r);
      return n;
    }
    function k(n) {
      return n.then;
    }
    function x(n, t, e) {
      var r = c(e, t)(
        function (e) {
          t && ((t = S), g(n, e));
        },
        function (e) {
          t && ((t = S), y(n, O, e));
        }
      );
      r === Y && t && (y(n, O, r.e), (t = S));
    }
    var T,
      b,
      R,
      S = null,
      C = "object" == typeof self,
      F = self,
      P = F.Promise,
      E = F.process,
      H = F.console,
      I = !0,
      L = Array,
      A = Error,
      O = 1,
      U = 2,
      $ = 3,
      q = "Symbol",
      z = "iterator",
      B = "species",
      D = q + "(" + B + ")",
      G = "return",
      J = "_uh",
      K = "_pt",
      M = "_st",
      N = "Invalid argument",
      Q = "\nFrom previous ",
      V = "Chaining cycle detected for promise",
      W = "rejectionHandled",
      X = "unhandledRejection",
      Y = { e: S },
      Z = function () {},
      nn = /^.+\/node_modules\/yaku\/.+\n?/gm,
      tn = function (n) {
        var r,
          o = this;
        if (!t(o) || o._s !== T) throw a("Invalid this");
        if (((o._s = $), I && (o[K] = l()), n !== Z)) {
          if (!e(n)) throw a(N);
          (r = c(n)(d(o, U), d(o, O))), r === Y && y(o, O, r.e);
        }
      };
    (tn["default"] = tn),
      (function en(n, t) {
        for (var e in t) n[e] = t[e];
      })(tn.prototype, {
        then: function (n, t) {
          if (this._s === undefined) throw a();
          return p(this, _(tn.speciesConstructor(this, tn)), n, t);
        },
        catch: function (n) {
          return this.then(T, n);
        },
        finally: function (n) {
          return this.then(
            function (t) {
              return tn.resolve(n()).then(function () {
                return t;
              });
            },
            function (t) {
              return tn.resolve(n()).then(function () {
                throw t;
              });
            }
          );
        },
        _c: 0,
        _p: S,
      }),
      (tn.resolve = function (n) {
        return v(n) ? n : g(_(this), n);
      }),
      (tn.reject = function (n) {
        return y(_(this), O, n);
      }),
      (tn.race = function (n) {
        var t = this,
          e = _(t),
          r = function (n) {
            y(e, U, n);
          },
          o = function (n) {
            y(e, O, n);
          },
          i = c(s)(n, function (n) {
            t.resolve(n).then(r, o);
          });
        return i === Y ? t.reject(i.e) : e;
      }),
      (tn.all = function (n) {
        function t(n) {
          y(o, O, n);
        }
        var e,
          r = this,
          o = _(r),
          i = [];
        return (e = c(s)(n, function (n, u) {
          r.resolve(n).then(function (n) {
            (i[u] = n), --e || y(o, U, i);
          }, t);
        })) === Y
          ? r.reject(e.e)
          : (e || y(o, U, []), o);
      }),
      (tn.Symbol = F[q] || {}),
      c(function () {
        Object.defineProperty(tn, n(), {
          get: function () {
            return this;
          },
        });
      })(),
      (tn.speciesConstructor = function (t, e) {
        var r = t.constructor;
        return r ? r[n()] || e : e;
      }),
      (tn.unhandledRejection = function (n, t) {
        H && H.error("Uncaught (in promise)", I ? t.longStack : w(n, t));
      }),
      (tn.rejectionHandled = Z),
      (tn.enableLongStackTrace = function () {
        I = !0;
      }),
      (tn.nextTick = C
        ? function (n) {
            P
              ? new P(function (n) {
                  n();
                }).then(n)
              : setTimeout(n);
          }
        : E.nextTick),
      (tn._s = 1);
    var rn = f(999, function (n, t) {
        var e, r;
        return (r = n._s !== O ? t._onFulfilled : t._onRejected) === T
          ? void y(t, n._s, n._v)
          : (e = c(j)(r, n._v)) === Y
          ? void y(t, O, e.e)
          : void g(t, e);
      }),
      on = f(9, function (n) {
        m(n) || ((n[J] = 1), h(X, n));
      });
    F.Promise = tn;
  })();
  !(function (e) {
    "use strict";
    function t(t) {
      return (
        !!t &&
        (("Symbol" in e &&
          "iterator" in e.Symbol &&
          "function" == typeof t[Symbol.iterator]) ||
          !!Array.isArray(t))
      );
    }
    !(function () {
      function n(e) {
        var t = "",
          n = !0;
        return (
          e.forEach(function (e) {
            var r = encodeURIComponent(e.name),
              a = encodeURIComponent(e.value);
            n || (t += "&"), (t += r + "=" + a), (n = !1);
          }),
          t.replace(/%20/g, "+")
        );
      }
      function r(e) {
        return e.replace(/((%[0-9A-Fa-f]{2})*)/g, function (e, t) {
          return decodeURIComponent(t);
        });
      }
      function a(e, t) {
        var n = e.split("&");
        t && -1 === n[0].indexOf("=") && (n[0] = "=" + n[0]);
        var a = [];
        n.forEach(function (e) {
          if (0 !== e.length) {
            var t = e.indexOf("=");
            if (-1 !== t)
              var n = e.substring(0, t),
                r = e.substring(t + 1);
            else (n = e), (r = "");
            (n = n.replace(/\+/g, " ")),
              (r = r.replace(/\+/g, " ")),
              a.push({ name: n, value: r });
          }
        });
        var i = [];
        return (
          a.forEach(function (e) {
            i.push({ name: r(e.name), value: r(e.value) });
          }),
          i
        );
      }
      function i(e) {
        if (c) return new s(e);
        var t = document.createElement("a");
        return (t.href = e), t;
      }
      function o(e) {
        var r = this;
        (this._list = []),
          e === undefined ||
            null === e ||
            (e instanceof o
              ? (this._list = a(String(e)))
              : "object" == typeof e && t(e)
              ? Array.from(e).forEach(function (e) {
                  if (!t(e)) throw TypeError();
                  var n = Array.from(e);
                  if (2 !== n.length) throw TypeError();
                  r._list.push({ name: String(n[0]), value: String(n[1]) });
                })
              : "object" == typeof e && e
              ? Object.keys(e).forEach(function (t) {
                  r._list.push({ name: String(t), value: String(e[t]) });
                })
              : ((e = String(e)),
                "?" === e.substring(0, 1) && (e = e.substring(1)),
                (this._list = a(e)))),
          (this._url_object = null),
          (this._setList = function (e) {
            i || (r._list = e);
          });
        var i = !1;
        this._update_steps = function () {
          i ||
            ((i = !0),
            r._url_object &&
              ("about:" === r._url_object.protocol &&
                -1 !== r._url_object.pathname.indexOf("?") &&
                (r._url_object.pathname = r._url_object.pathname.split("?")[0]),
              (r._url_object.search = n(r._list)),
              (i = !1)));
        };
      }
      function u(e, t) {
        var n = 0;
        this.next = function () {
          if (n >= e.length) return { done: !0, value: undefined };
          var r = e[n++];
          return {
            done: !1,
            value:
              "key" === t
                ? r.name
                : "value" === t
                ? r.value
                : [r.name, r.value],
          };
        };
      }
      function l(t, n) {
        function r() {
          var e = l.href.replace(/#$|\?$|\?(?=#)/g, "");
          l.href !== e && (l.href = e);
        }
        function u() {
          m._setList(l.search ? a(l.search.substring(1)) : []),
            m._update_steps();
        }
        if (!(this instanceof e.URL))
          throw new TypeError(
            "Failed to construct 'URL': Please use the 'new' operator."
          );
        n &&
          (t = (function () {
            if (c) return new s(t, n).href;
            var e;
            try {
              var r;
              if (
                ("[object OperaMini]" ===
                Object.prototype.toString.call(window.operamini)
                  ? ((e = document.createElement("iframe")),
                    (e.style.display = "none"),
                    document.documentElement.appendChild(e),
                    (r = e.contentWindow.document))
                  : document.implementation &&
                    document.implementation.createHTMLDocument
                  ? (r = document.implementation.createHTMLDocument(""))
                  : document.implementation &&
                    document.implementation.createDocument
                  ? ((r = document.implementation.createDocument(
                      "http://www.w3.org/1999/xhtml",
                      "html",
                      null
                    )),
                    r.documentElement.appendChild(r.createElement("head")),
                    r.documentElement.appendChild(r.createElement("body")))
                  : window.ActiveXObject &&
                    ((r = new window.ActiveXObject("htmlfile")),
                    r.write("<head></head><body></body>"),
                    r.close()),
                !r)
              )
                throw Error("base not supported");
              var a = r.createElement("base");
              (a.href = n), r.getElementsByTagName("head")[0].appendChild(a);
              var i = r.createElement("a");
              return (i.href = t), i.href;
            } finally {
              e && e.parentNode.removeChild(e);
            }
          })());
        var l = i(t || ""),
          f = (function () {
            if (!("defineProperties" in Object)) return !1;
            try {
              var e = {};
              return (
                Object.defineProperties(e, {
                  prop: {
                    get: function () {
                      return !0;
                    },
                  },
                }),
                e.prop
              );
            } catch (t) {
              return !1;
            }
          })(),
          h = f ? this : document.createElement("a"),
          m = new o(l.search ? l.search.substring(1) : null);
        return (
          (m._url_object = h),
          Object.defineProperties(h, {
            href: {
              get: function () {
                return l.href;
              },
              set: function (e) {
                (l.href = e), r(), u();
              },
              enumerable: !0,
              configurable: !0,
            },
            origin: {
              get: function () {
                return "data:" === this.protocol.toLowerCase()
                  ? null
                  : "origin" in l
                  ? l.origin
                  : this.protocol + "//" + this.host;
              },
              enumerable: !0,
              configurable: !0,
            },
            protocol: {
              get: function () {
                return l.protocol;
              },
              set: function (e) {
                l.protocol = e;
              },
              enumerable: !0,
              configurable: !0,
            },
            username: {
              get: function () {
                return l.username;
              },
              set: function (e) {
                l.username = e;
              },
              enumerable: !0,
              configurable: !0,
            },
            password: {
              get: function () {
                return l.password;
              },
              set: function (e) {
                l.password = e;
              },
              enumerable: !0,
              configurable: !0,
            },
            host: {
              get: function () {
                var e = { "http:": /:80$/, "https:": /:443$/, "ftp:": /:21$/ }[
                  l.protocol
                ];
                return e ? l.host.replace(e, "") : l.host;
              },
              set: function (e) {
                l.host = e;
              },
              enumerable: !0,
              configurable: !0,
            },
            hostname: {
              get: function () {
                return l.hostname;
              },
              set: function (e) {
                l.hostname = e;
              },
              enumerable: !0,
              configurable: !0,
            },
            port: {
              get: function () {
                return l.port;
              },
              set: function (e) {
                l.port = e;
              },
              enumerable: !0,
              configurable: !0,
            },
            pathname: {
              get: function () {
                return "/" !== l.pathname.charAt(0)
                  ? "/" + l.pathname
                  : l.pathname;
              },
              set: function (e) {
                l.pathname = e;
              },
              enumerable: !0,
              configurable: !0,
            },
            search: {
              get: function () {
                return l.search;
              },
              set: function (e) {
                l.search !== e && ((l.search = e), r(), u());
              },
              enumerable: !0,
              configurable: !0,
            },
            searchParams: {
              get: function () {
                return m;
              },
              enumerable: !0,
              configurable: !0,
            },
            hash: {
              get: function () {
                return l.hash;
              },
              set: function (e) {
                (l.hash = e), r();
              },
              enumerable: !0,
              configurable: !0,
            },
            toString: {
              value: function () {
                return l.toString();
              },
              enumerable: !1,
              configurable: !0,
            },
            valueOf: {
              value: function () {
                return l.valueOf();
              },
              enumerable: !1,
              configurable: !0,
            },
          }),
          h
        );
      }
      var c,
        s = e.URL;
      try {
        if (s) {
          if ("searchParams" in (c = new e.URL("http://example.com"))) {
            var f = new l("http://example.com");
            if (
              ((f.search = "a=1&b=2"),
              "http://example.com/?a=1&b=2" === f.href &&
                ((f.search = ""), "http://example.com/" === f.href))
            )
              return;
          }
          "href" in c || (c = undefined), (c = undefined);
        }
      } catch (m) {}
      if (
        (Object.defineProperties(o.prototype, {
          append: {
            value: function (e, t) {
              this._list.push({ name: e, value: t }), this._update_steps();
            },
            writable: !0,
            enumerable: !0,
            configurable: !0,
          },
          delete: {
            value: function (e) {
              for (var t = 0; t < this._list.length; )
                this._list[t].name === e ? this._list.splice(t, 1) : ++t;
              this._update_steps();
            },
            writable: !0,
            enumerable: !0,
            configurable: !0,
          },
          get: {
            value: function (e) {
              for (var t = 0; t < this._list.length; ++t)
                if (this._list[t].name === e) return this._list[t].value;
              return null;
            },
            writable: !0,
            enumerable: !0,
            configurable: !0,
          },
          getAll: {
            value: function (e) {
              for (var t = [], n = 0; n < this._list.length; ++n)
                this._list[n].name === e && t.push(this._list[n].value);
              return t;
            },
            writable: !0,
            enumerable: !0,
            configurable: !0,
          },
          has: {
            value: function (e) {
              for (var t = 0; t < this._list.length; ++t)
                if (this._list[t].name === e) return !0;
              return !1;
            },
            writable: !0,
            enumerable: !0,
            configurable: !0,
          },
          set: {
            value: function (e, t) {
              for (var n = !1, r = 0; r < this._list.length; )
                this._list[r].name === e
                  ? n
                    ? this._list.splice(r, 1)
                    : ((this._list[r].value = t), (n = !0), ++r)
                  : ++r;
              n || this._list.push({ name: e, value: t }), this._update_steps();
            },
            writable: !0,
            enumerable: !0,
            configurable: !0,
          },
          entries: {
            value: function () {
              return new u(this._list, "key+value");
            },
            writable: !0,
            enumerable: !0,
            configurable: !0,
          },
          keys: {
            value: function () {
              return new u(this._list, "key");
            },
            writable: !0,
            enumerable: !0,
            configurable: !0,
          },
          values: {
            value: function () {
              return new u(this._list, "value");
            },
            writable: !0,
            enumerable: !0,
            configurable: !0,
          },
          forEach: {
            value: function (e) {
              var t = arguments.length > 1 ? arguments[1] : undefined;
              this._list.forEach(function (n) {
                e.call(t, n.value, n.name);
              });
            },
            writable: !0,
            enumerable: !0,
            configurable: !0,
          },
          toString: {
            value: function () {
              return n(this._list);
            },
            writable: !0,
            enumerable: !1,
            configurable: !0,
          },
          sort: {
            value: function p() {
              for (
                var e = this.entries(), t = e.next(), n = [], r = {};
                !t.done;

              ) {
                var a = t.value,
                  i = a[0];
                n.push(i),
                  Object.prototype.hasOwnProperty.call(r, i) || (r[i] = []),
                  r[i].push(a[1]),
                  (t = e.next());
              }
              n.sort();
              for (var o = 0; o < n.length; o++) this["delete"](n[o]);
              for (var u = 0; u < n.length; u++)
                (i = n[u]), this.append(i, r[i].shift());
            },
          },
        }),
        "Symbol" in e &&
          "iterator" in e.Symbol &&
          (Object.defineProperty(o.prototype, e.Symbol.iterator, {
            value: o.prototype.entries,
            writable: !0,
            enumerable: !0,
            configurable: !0,
          }),
          Object.defineProperty(u.prototype, e.Symbol.iterator, {
            value: function () {
              return this;
            },
            writable: !0,
            enumerable: !0,
            configurable: !0,
          })),
        s)
      )
        for (var h in s)
          Object.prototype.hasOwnProperty.call(s, h) &&
            "function" == typeof s[h] &&
            (l[h] = s[h]);
      (e.URL = l), (e.URLSearchParams = o);
    })(),
      (function () {
        if (
          "1" !== new e.URLSearchParams([["a", 1]]).get("a") ||
          "1" !== new e.URLSearchParams({ a: 1 }).get("a")
        ) {
          var n = e.URLSearchParams;
          e.URLSearchParams = function (e) {
            if (e && "object" == typeof e && t(e)) {
              var r = new n();
              return (
                Array.from(e).forEach(function (e) {
                  if (!t(e)) throw TypeError();
                  var n = Array.from(e);
                  if (2 !== n.length) throw TypeError();
                  r.append(n[0], n[1]);
                }),
                r
              );
            }
            return e && "object" == typeof e
              ? ((r = new n()),
                Object.keys(e).forEach(function (t) {
                  r.set(t, e[t]);
                }),
                r)
              : new n(e);
          };
        }
      })();
  })(self);
  !(function (t, e) {
    "object" == typeof exports && "undefined" != typeof module
      ? e(exports)
      : "function" == typeof define && define.amd
      ? define(["exports"], e)
      : e((t.WHATWGFetch = {}));
  })(this, function (t) {
    "use strict";
    function e(t) {
      return t && DataView.prototype.isPrototypeOf(t);
    }
    function r(t) {
      if (
        ("string" != typeof t && (t = String(t)),
        /[^a-z0-9\-#$%&'*+.^_`|~!]/i.test(t) || "" === t)
      )
        throw new TypeError(
          'Invalid character in header field name: "' + t + '"'
        );
      return t.toLowerCase();
    }
    function o(t) {
      return "string" != typeof t && (t = String(t)), t;
    }
    function n(t) {
      var e = {
        next: function () {
          var e = t.shift();
          return { done: e === undefined, value: e };
        },
      };
      return (
        E.iterable &&
          (e[Symbol.iterator] = function () {
            return e;
          }),
        e
      );
    }
    function i(t) {
      (this.map = {}),
        t instanceof i
          ? t.forEach(function (t, e) {
              this.append(e, t);
            }, this)
          : Array.isArray(t)
          ? t.forEach(function (t) {
              this.append(t[0], t[1]);
            }, this)
          : t &&
            Object.getOwnPropertyNames(t).forEach(function (e) {
              this.append(e, t[e]);
            }, this);
    }
    function s(t) {
      if (t.bodyUsed) return Promise.reject(new TypeError("Already read"));
      t.bodyUsed = !0;
    }
    function a(t) {
      return new Promise(function (e, r) {
        (t.onload = function () {
          e(t.result);
        }),
          (t.onerror = function () {
            r(t.error);
          });
      });
    }
    function f(t) {
      var e = new FileReader(),
        r = a(e);
      return e.readAsArrayBuffer(t), r;
    }
    function u(t) {
      var e = new FileReader(),
        r = a(e);
      return e.readAsText(t), r;
    }
    function h(t) {
      for (
        var e = new Uint8Array(t), r = new Array(e.length), o = 0;
        o < e.length;
        o++
      )
        r[o] = String.fromCharCode(e[o]);
      return r.join("");
    }
    function c(t) {
      if (t.slice) return t.slice(0);
      var e = new Uint8Array(t.byteLength);
      return e.set(new Uint8Array(t)), e.buffer;
    }
    function d() {
      return (
        (this.bodyUsed = !1),
        (this._initBody = function (t) {
          (this.bodyUsed = this.bodyUsed),
            (this._bodyInit = t),
            t
              ? "string" == typeof t
                ? (this._bodyText = t)
                : E.blob && Blob.prototype.isPrototypeOf(t)
                ? (this._bodyBlob = t)
                : E.formData && FormData.prototype.isPrototypeOf(t)
                ? (this._bodyFormData = t)
                : E.searchParams && URLSearchParams.prototype.isPrototypeOf(t)
                ? (this._bodyText = t.toString())
                : E.arrayBuffer && E.blob && e(t)
                ? ((this._bodyArrayBuffer = c(t.buffer)),
                  (this._bodyInit = new Blob([this._bodyArrayBuffer])))
                : E.arrayBuffer &&
                  (ArrayBuffer.prototype.isPrototypeOf(t) || A(t))
                ? (this._bodyArrayBuffer = c(t))
                : (this._bodyText = t = Object.prototype.toString.call(t))
              : (this._bodyText = ""),
            this.headers.get("content-type") ||
              ("string" == typeof t
                ? this.headers.set("content-type", "text/plain;charset=UTF-8")
                : this._bodyBlob && this._bodyBlob.type
                ? this.headers.set("content-type", this._bodyBlob.type)
                : E.searchParams &&
                  URLSearchParams.prototype.isPrototypeOf(t) &&
                  this.headers.set(
                    "content-type",
                    "application/x-www-form-urlencoded;charset=UTF-8"
                  ));
        }),
        E.blob &&
          ((this.blob = function () {
            var t = s(this);
            if (t) return t;
            if (this._bodyBlob) return Promise.resolve(this._bodyBlob);
            if (this._bodyArrayBuffer)
              return Promise.resolve(new Blob([this._bodyArrayBuffer]));
            if (this._bodyFormData)
              throw new Error("could not read FormData body as blob");
            return Promise.resolve(new Blob([this._bodyText]));
          }),
          (this.arrayBuffer = function () {
            if (this._bodyArrayBuffer) {
              var t = s(this);
              return (
                t ||
                (ArrayBuffer.isView(this._bodyArrayBuffer)
                  ? Promise.resolve(
                      this._bodyArrayBuffer.buffer.slice(
                        this._bodyArrayBuffer.byteOffset,
                        this._bodyArrayBuffer.byteOffset +
                          this._bodyArrayBuffer.byteLength
                      )
                    )
                  : Promise.resolve(this._bodyArrayBuffer))
              );
            }
            return this.blob().then(f);
          })),
        (this.text = function () {
          var t = s(this);
          if (t) return t;
          if (this._bodyBlob) return u(this._bodyBlob);
          if (this._bodyArrayBuffer)
            return Promise.resolve(h(this._bodyArrayBuffer));
          if (this._bodyFormData)
            throw new Error("could not read FormData body as text");
          return Promise.resolve(this._bodyText);
        }),
        E.formData &&
          (this.formData = function () {
            return this.text().then(l);
          }),
        (this.json = function () {
          return this.text().then(JSON.parse);
        }),
        this
      );
    }
    function y(t) {
      var e = t.toUpperCase();
      return _.indexOf(e) > -1 ? e : t;
    }
    function p(t, e) {
      if (!(this instanceof p))
        throw new TypeError(
          'Please use the "new" operator, this DOM object constructor cannot be called as a function.'
        );
      e = e || {};
      var r = e.body;
      if (t instanceof p) {
        if (t.bodyUsed) throw new TypeError("Already read");
        (this.url = t.url),
          (this.credentials = t.credentials),
          e.headers || (this.headers = new i(t.headers)),
          (this.method = t.method),
          (this.mode = t.mode),
          (this.signal = t.signal),
          r || null == t._bodyInit || ((r = t._bodyInit), (t.bodyUsed = !0));
      } else this.url = String(t);
      if (
        ((this.credentials =
          e.credentials || this.credentials || "same-origin"),
        (!e.headers && this.headers) || (this.headers = new i(e.headers)),
        (this.method = y(e.method || this.method || "GET")),
        (this.mode = e.mode || this.mode || null),
        (this.signal = e.signal || this.signal),
        (this.referrer = null),
        ("GET" === this.method || "HEAD" === this.method) && r)
      )
        throw new TypeError("Body not allowed for GET or HEAD requests");
      if (
        (this._initBody(r),
        !(
          ("GET" !== this.method && "HEAD" !== this.method) ||
          ("no-store" !== e.cache && "no-cache" !== e.cache)
        ))
      ) {
        var o = /([?&])_=[^&]*/;
        if (o.test(this.url))
          this.url = this.url.replace(o, "$1_=" + new Date().getTime());
        else {
          var n = /\?/;
          this.url +=
            (n.test(this.url) ? "&" : "?") + "_=" + new Date().getTime();
        }
      }
    }
    function l(t) {
      var e = new FormData();
      return (
        t
          .trim()
          .split("&")
          .forEach(function (t) {
            if (t) {
              var r = t.split("="),
                o = r.shift().replace(/\+/g, " "),
                n = r.join("=").replace(/\+/g, " ");
              e.append(decodeURIComponent(o), decodeURIComponent(n));
            }
          }),
        e
      );
    }
    function b(t) {
      var e = new i();
      return (
        t
          .replace(/\r?\n[\t ]+/g, " ")
          .split("\r")
          .map(function (t) {
            return 0 === t.indexOf("\n") ? t.substr(1, t.length) : t;
          })
          .forEach(function (t) {
            var r = t.split(":"),
              o = r.shift().trim();
            if (o) {
              var n = r.join(":").trim();
              e.append(o, n);
            }
          }),
        e
      );
    }
    function m(t, e) {
      if (!(this instanceof m))
        throw new TypeError(
          'Please use the "new" operator, this DOM object constructor cannot be called as a function.'
        );
      e || (e = {}),
        (this.type = "default"),
        (this.status = e.status === undefined ? 200 : e.status),
        (this.ok = this.status >= 200 && this.status < 300),
        (this.statusText = e.statusText === undefined ? "" : "" + e.statusText),
        (this.headers = new i(e.headers)),
        (this.url = e.url || ""),
        this._initBody(t);
    }
    function w(e, r) {
      return new Promise(function (n, s) {
        function a() {
          u.abort();
        }
        var f = new p(e, r);
        if (f.signal && f.signal.aborted)
          return s(new t.DOMException("Aborted", "AbortError"));
        var u = new XMLHttpRequest();
        (u.onload = function () {
          var t = {
            status: u.status,
            statusText: u.statusText,
            headers: b(u.getAllResponseHeaders() || ""),
          };
          t.url =
            "responseURL" in u ? u.responseURL : t.headers.get("X-Request-URL");
          var e = "response" in u ? u.response : u.responseText;
          setTimeout(function () {
            n(new m(e, t));
          }, 0);
        }),
          (u.onerror = function () {
            setTimeout(function () {
              s(new TypeError("Network request failed"));
            }, 0);
          }),
          (u.ontimeout = function () {
            setTimeout(function () {
              s(new TypeError("Network request failed"));
            }, 0);
          }),
          (u.onabort = function () {
            setTimeout(function () {
              s(new t.DOMException("Aborted", "AbortError"));
            }, 0);
          }),
          u.open(
            f.method,
            (function h(t) {
              try {
                return "" === t && v.location.href ? v.location.href : t;
              } catch (e) {
                return t;
              }
            })(f.url),
            !0
          ),
          "include" === f.credentials
            ? (u.withCredentials = !0)
            : "omit" === f.credentials && (u.withCredentials = !1),
          "responseType" in u &&
            (E.blob
              ? (u.responseType = "blob")
              : E.arrayBuffer &&
                f.headers.get("Content-Type") &&
                -1 !==
                  f.headers
                    .get("Content-Type")
                    .indexOf("application/octet-stream") &&
                (u.responseType = "arraybuffer")),
          !r || "object" != typeof r.headers || r.headers instanceof i
            ? f.headers.forEach(function (t, e) {
                u.setRequestHeader(e, t);
              })
            : Object.getOwnPropertyNames(r.headers).forEach(function (t) {
                u.setRequestHeader(t, o(r.headers[t]));
              }),
          f.signal &&
            (f.signal.addEventListener("abort", a),
            (u.onreadystatechange = function () {
              4 === u.readyState && f.signal.removeEventListener("abort", a);
            })),
          u.send("undefined" == typeof f._bodyInit ? null : f._bodyInit);
      });
    }
    var v =
        ("undefined" != typeof globalThis && globalThis) ||
        ("undefined" != typeof self && self) ||
        (void 0 !== v && v),
      E = {
        searchParams: "URLSearchParams" in v,
        iterable: "Symbol" in v && "iterator" in Symbol,
        blob:
          "FileReader" in v &&
          "Blob" in v &&
          (function () {
            try {
              return new Blob(), !0;
            } catch (t) {
              return !1;
            }
          })(),
        formData: "FormData" in v,
        arrayBuffer: "ArrayBuffer" in v,
      };
    if (E.arrayBuffer)
      var T = [
          "[object Int8Array]",
          "[object Uint8Array]",
          "[object Uint8ClampedArray]",
          "[object Int16Array]",
          "[object Uint16Array]",
          "[object Int32Array]",
          "[object Uint32Array]",
          "[object Float32Array]",
          "[object Float64Array]",
        ],
        A =
          ArrayBuffer.isView ||
          function (t) {
            return t && T.indexOf(Object.prototype.toString.call(t)) > -1;
          };
    (i.prototype.append = function (t, e) {
      (t = r(t)), (e = o(e));
      var n = this.map[t];
      this.map[t] = n ? n + ", " + e : e;
    }),
      (i.prototype["delete"] = function (t) {
        delete this.map[r(t)];
      }),
      (i.prototype.get = function (t) {
        return (t = r(t)), this.has(t) ? this.map[t] : null;
      }),
      (i.prototype.has = function (t) {
        return this.map.hasOwnProperty(r(t));
      }),
      (i.prototype.set = function (t, e) {
        this.map[r(t)] = o(e);
      }),
      (i.prototype.forEach = function (t, e) {
        for (var r in this.map)
          this.map.hasOwnProperty(r) && t.call(e, this.map[r], r, this);
      }),
      (i.prototype.keys = function () {
        var t = [];
        return (
          this.forEach(function (e, r) {
            t.push(r);
          }),
          n(t)
        );
      }),
      (i.prototype.values = function () {
        var t = [];
        return (
          this.forEach(function (e) {
            t.push(e);
          }),
          n(t)
        );
      }),
      (i.prototype.entries = function () {
        var t = [];
        return (
          this.forEach(function (e, r) {
            t.push([r, e]);
          }),
          n(t)
        );
      }),
      E.iterable && (i.prototype[Symbol.iterator] = i.prototype.entries);
    var _ = ["DELETE", "GET", "HEAD", "OPTIONS", "POST", "PUT"];
    (p.prototype.clone = function () {
      return new p(this, { body: this._bodyInit });
    }),
      d.call(p.prototype),
      d.call(m.prototype),
      (m.prototype.clone = function () {
        return new m(this._bodyInit, {
          status: this.status,
          statusText: this.statusText,
          headers: new i(this.headers),
          url: this.url,
        });
      }),
      (m.error = function () {
        var t = new m(null, { status: 0, statusText: "" });
        return (t.type = "error"), t;
      });
    var g = [301, 302, 303, 307, 308];
    (m.redirect = function (t, e) {
      if (-1 === g.indexOf(e)) throw new RangeError("Invalid status code");
      return new m(null, { status: e, headers: { location: t } });
    }),
      (t.DOMException = v.DOMException);
    try {
      new t.DOMException();
    } catch (B) {
      (t.DOMException = function (t, e) {
        (this.message = t), (this.name = e);
        var r = Error(t);
        this.stack = r.stack;
      }),
        (t.DOMException.prototype = Object.create(Error.prototype)),
        (t.DOMException.prototype.constructor = t.DOMException);
    }
    (w.polyfill = !0),
      (v.fetch = w),
      (v.Headers = i),
      (v.Request = p),
      (v.Response = m),
      (t.Headers = i),
      (t.Request = p),
      (t.Response = m),
      (t.fetch = w),
      Object.defineProperty(t, "__esModule", { value: !0 });
  });
})(
  ("object" === typeof window && window) ||
    ("object" === typeof self && self) ||
    ("object" === typeof global && global) ||
    {}
);
