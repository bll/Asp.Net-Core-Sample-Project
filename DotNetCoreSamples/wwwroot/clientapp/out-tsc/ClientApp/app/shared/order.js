"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var _ = require("lodash"); // sum vb metodların hazır bulunduğu kütüphane _ ile kullanmak zorunlu :)
var Order = (function () {
    function Order() {
        this.orderDate = new Date();
        this.items = new Array();
    }
    Object.defineProperty(Order.prototype, "subtotal", {
        get: function () {
            return _.sum(_.map(this.items, function (i) { return i.unitPrice * i.quantity; }));
        },
        enumerable: true,
        configurable: true
    });
    ;
    return Order;
}());
exports.Order = Order;
var OrderItem = (function () {
    function OrderItem() {
    }
    return OrderItem;
}());
exports.OrderItem = OrderItem;
//# sourceMappingURL=order.js.map