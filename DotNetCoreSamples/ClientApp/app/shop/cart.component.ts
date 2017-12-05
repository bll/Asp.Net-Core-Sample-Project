import { Component } from "@angular/core";
import { DataService } from "../shared/dataService";
import { Product } from "../shared/product";

@Component({
    selector: "the-cart",
    templateUrl: "cart.component.html",
    styleUrls: []
})

export class Cart {

    constructor(private data: DataService) {

    }


}