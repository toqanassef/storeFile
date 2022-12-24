import { Component } from '@angular/core';
import { StoreRetData } from './_models/store-ret-data';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Store';
  total = 0;
  remain = 0;
  tnumber = 0;

  FooterData(data:StoreRetData){
    this.total=data.amountTotal;
    this.remain=data.amountRemain;
    this.tnumber=data.transactionNum;
  }
}
