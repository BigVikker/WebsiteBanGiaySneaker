function floatToString(t, r) {
    var e = t.toFixed(r).toString();
    return e.match(/^\.\d+/) ? "0" + e : e
}

function attributeToString(t) {
    return "string" != typeof t && (t += "", "undefined" === t && (t = "")), jQuery.trim(t)
}

"undefined" == typeof Huudat && (Huudat = {});

Huudat.mediaDomainName = "//huudat.niceshose.net/";

Huudat.money_format = "${{amount}}", Huudat.onError = function (XMLHttpRequest, textStatus) {
    var data = eval("(" + XMLHttpRequest.responseText + ")");
    alert(data.message ? data.message + "(" + data.status + "): " + data.description : "Error : " + Huudat.fullMessagesFromErrors(data).join("; ") + ".")
}, Huudat.fullMessagesFromErrors = function (t) {
    var r = [];
    return jQuery.each(t, function (t, e) {
        jQuery.each(e, function (e, o) {
            r.push(t + " " + o)  
        })
    }), r
}, Huudat.onCartUpdate = function (t) {
    alert("There are now " + t.item_count + " items in the cart.")
}, Huudat.onCartShippingRatesUpdate = function (t, r) {
    var e = "";
    r.zip && (e += r.zip + ", "), r.province && (e += r.province + ", "), e += r.country, alert("There are " + t.length + " shipping rates available for " + e + ", starting at " + Huudat.formatMoney(t[0].price) + ".")
}, Huudat.onItemAdded = function (t) {
    alert(t.title + " was added to your shopping cart.")
}, Huudat.onProduct = function (t) {
    alert("Received everything we ever wanted to know about " + t.title)
}, Huudat.formatMoney = function (amount, moneyFormat) {
    function getDefault(value, defaultValue) {
        if (typeof value == "undefined")
            return defaultValue;

        return value;
    }

    function formatMoney(amount, decimal, thousandSeperate, decimalSeperate) {
        decimal = getDefault(decimal, 2);
        thousandSeperate = getDefault(thousandSeperate, ",");
        decimalSeperate = getDefault(decimalSeperate, ".");

        if (isNaN(amount) || null == amount)
            return 0;

        amount = amount.toFixed(decimal);

        var amountParts = amount.split(".");
        var integer = amountParts[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1" + thousandSeperate);
        var decimal = amountParts[1] ? decimalSeperate + amountParts[1] : "";

        return integer + decimal;
    }

    if (typeof amount == "string") {
        amount = amount.replace(".", "");
        amount = amount.replace(",", "");
    }

    var result = "";
    var moneyRegex = /\{\{\s*(\w+)\s*\}\}/;

    moneyFormat = moneyFormat || this.money_format;
    switch (moneyFormat.match(moneyRegex)[1]) {
        case "amount":
            result = formatMoney(amount, 2);
            break;
        case "amount_no_decimals":
            result = formatMoney(amount, 0);
            break;
        case "amount_with_comma_separator":
            result = formatMoney(amount, 2, ".", ",");
            break;
        case "amount_no_decimals_with_comma_separator":
            result = formatMoney(amount, 0, ".", ",")
    }

    return moneyFormat.replace(moneyRegex, result)
}, Huudat.resizeImage = function (t, r) {
    try {
        if ("original" == r)
            return t;

        var thumbDomain = Huudat.mediaDomainName + "thumb/" + r + "/";
        return t.replace(Huudat.mediaDomainName, thumbDomain).split('?')[0];
    } catch (o) {
        return t
    }
}, Huudat.addItem = function (t, r, e) {
    var r = r || 1,
        o = {
            type: "POST",
            url: "/cart/add.js",
            data: "quantity=" + r + "&VariantId=" + t,
            dataType: "json",
            success: function (t) {
                "function" == typeof e ? e(t) : Huudat.onItemAdded(t)
            },
            error: function (t, r) {
                Huudat.onError(t, r)
            }
        };
    jQuery.ajax(o)
}, Huudat.addItemFromForm = function (t, r) {
    var e = {
        type: "POST",
        url: "/cart/add.js",
        data: jQuery("#" + t).serialize(),
        dataType: "json",
        success: function (t) {
            "function" == typeof r ? r(t) : Huudat.onItemAdded(t)
        },
        error: function (t, r) {
            Huudat.onError(t, r)
        }
    };
    jQuery.ajax(e)
}, Huudat.getCart = function (t) {
    jQuery.getJSON("/cart.js", function (r) {
        "function" == typeof t ? t(r) : Huudat.onCartUpdate(r)
    })
}, Huudat.pollForCartShippingRatesForDestination = function (t, r, e) {
    e = e || Huudat.onError;
    var o = function () {
        jQuery.ajax("/cart/async_shipping_rates", {
            dataType: "json",
            success: function (e, n, a) {
                200 === a.status ? "function" == typeof r ? r(e.shipping_rates, t) : Huudat.onCartShippingRatesUpdate(e.shipping_rates, t) : setTimeout(o, 500)
            },
            error: e
        })
    };
    return o
}, Huudat.getCartShippingRatesForDestination = function (t, r, e) {
    e = e || Huudat.onError;
    var o = {
        type: "POST",
        url: "/cart/prepare_shipping_rates",
        data: Huudat.param({
            shipping_address: t
        }),
        success: Huudat.pollForCartShippingRatesForDestination(t, r, e),
        error: e
    };
    jQuery.ajax(o)
}, Huudat.getProduct = function (t, r) {
    jQuery.getJSON("/products/" + t + ".js", function (t) {
        "function" == typeof r ? r(t) : Huudat.onProduct(t)
    })
}, Huudat.changeItem = function (t, r, e) {
    var o = {
        type: "POST",
        url: "/cart/change.js",
        data: "quantity=" + r + "&variantId=" + t,
        dataType: "json",
        success: function (t) {
            "function" == typeof e ? e(t) : Huudat.onCartUpdate(t)
        },
        error: function (t, r) {
            Huudat.onError(t, r)
        }
    };
    jQuery.ajax(o)
}, Huudat.removeItem = function (t, r) {
    var e = {
        type: "POST",
        url: "/cart/change.js",
        data: "quantity=0&variantId=" + t,
        dataType: "json",
        success: function (t) {
            "function" == typeof r ? r(t) : Huudat.onCartUpdate(t)
        },
        error: function (t, r) {
            Huudat.onError(t, r)
        }
    };
    jQuery.ajax(e)
}, Huudat.clear = function (t) {
    var r = {
        type: "POST",
        url: "/cart/clear.js",
        data: "",
        dataType: "json",
        success: function (r) {
            "function" == typeof t ? t(r) : Huudat.onCartUpdate(r)
        },
        error: function (t, r) {
            Huudat.onError(t, r)
        }
    };
    jQuery.ajax(r)
}, Huudat.updateCartFromForm = function (t, r) {
    var e = {
        type: "POST",
        url: "/cart/update.js",
        data: jQuery("#" + t).serialize(),
        dataType: "json",
        success: function (t) {
            "function" == typeof r ? r(t) : Huudat.onCartUpdate(t)
        },
        error: function (t, r) {
            Huudat.onError(t, r)
        }
    };
    jQuery.ajax(e)
}, Huudat.updateCartAttributes = function (t, r) {
    var e = "";
    jQuery.isArray(t) ? jQuery.each(t, function (t, r) {
        var o = attributeToString(r.key);
        "" !== o && (e += "attributes[" + o + "]=" + attributeToString(r.value) + "&")
    }) : "object" == typeof t && null !== t && jQuery.each(t, function (t, r) {
        e += "attributes[" + attributeToString(t) + "]=" + attributeToString(r) + "&"
    });
    var o = {
        type: "POST",
        url: "/cart/update.js",
        data: e,
        dataType: "json",
        success: function (t) {
            "function" == typeof r ? r(t) : Huudat.onCartUpdate(t)
        },
        error: function (t, r) {
            Huudat.onError(t, r)
        }
    };
    jQuery.ajax(o)
}, Huudat.updateCartNote = function (t, r) {
    var e = {
        type: "POST",
        url: "/cart/update.js",
        data: "note=" + attributeToString(t),
        dataType: "json",
        success: function (t) {
            "function" == typeof r ? r(t) : Huudat.onCartUpdate(t)
        },
        error: function (t, r) {
            Huudat.onError(t, r)
        }
    };
    jQuery.ajax(e)
}, jQuery.fn.jquery >= "1.4" ? Huudat.param = jQuery.param : (Huudat.param = function (t) {
    var r = [],
        e = function (t, e) {
            e = jQuery.isFunction(e) ? e() : e, r[r.length] = encodeURIComponent(t) + "=" + encodeURIComponent(e)
        };
    if (jQuery.isArray(t) || t.jquery) jQuery.each(t, function () {
        e(this.name, this.value)
    });
    else
        for (var o in t) Huudat.buildParams(o, t[o], e);
    return r.join("&").replace(/%20/g, "+")
}, Huudat.buildParams = function (t, r, e) {
    jQuery.isArray(r) && r.length ? jQuery.each(r, function (r, o) {
        rbracket.test(t) ? e(t, o) : Huudat.buildParams(t + "[" + ("object" == typeof o || jQuery.isArray(o) ? r : "") + "]", o, e)
    }) : null != r && "object" == typeof r ? Huudat.isEmptyObject(r) ? e(t, "") : jQuery.each(r, function (r, o) {
        Huudat.buildParams(t + "[" + r + "]", o, e)
    }) : e(t, r)
}, Huudat.isEmptyObject = function (t) {
    for (var r in t) return !1;
    return !0
});