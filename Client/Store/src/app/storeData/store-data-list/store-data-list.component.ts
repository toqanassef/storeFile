import { formatDate } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { StoreData } from 'src/app/_models/store-data';
import { StoreRetData } from 'src/app/_models/store-ret-data';
import { StoreformData } from 'src/app/_models/storeform-data';
import { StoreService } from 'src/app/_services/store.service';

@Component({
  selector: 'app-store-data-list',
  templateUrl: './store-data-list.component.html',
  styleUrls: ['./store-data-list.component.css']
})
export class StoreDataListComponent  {
  // event to update footer data after update table after submit form
  @Output() onChangeData : EventEmitter<StoreRetData> = new EventEmitter<StoreRetData>();

  retdata :StoreRetData = new StoreRetData(0,0,0,[]);
  dataList : StoreData[]=[];

  today:string =  formatDate(new Date(), 'yyyy-MM-dd','en');
  model = new StoreformData("2000-01-01",this.today,0);

  file: File[]=[];

  constructor(private StoreSer:StoreService){}

  // on form submit upload file to api and return data to update list in table
  save(){
    this.retdata  = new StoreRetData(0,0,0,[]);
    this.dataList =[];

    //console.log(f);
    this.StoreSer.GetStoreData(this.file[0], new StoreformData(this.model.fromDate,this.model.toDate,this.model.goodId))
    .subscribe(res=>{   
      // if sucess update data list and footer data by event
        this.dataList = res.goodData;
        this.retdata = res;

        this.onChangeData.emit(res);
        if(res.transactionNum==0)
        {
          alert("there no data in file match inputs");
        }
    }, error => {
      // if error clear data list and update footer data by zeros with event
      console.log(error);
      this.dataList = [];
      this.onChangeData.emit(new StoreRetData(0,0,0,[]));
      alert("Error Happened \n Please try again later or contact us");
    });
  }

  // on choose file in form update file property in component to sent it when submit firm
  onFilechange(event: any) {
    console.log(event.target.files[0]);
    this.file[0] = event.target.files[0];

  }

}


