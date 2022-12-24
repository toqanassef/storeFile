import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { StoreRetData } from '../_models/store-ret-data';
import { StoreformData } from '../_models/storeform-data';

@Injectable({
  providedIn: 'root'
})
export class StoreService {

  dataList :StoreRetData = new StoreRetData(0,0,0,[]);

  private baseUrl = 'https://localhost:44361/Store';
  constructor(private http:HttpClient) { }

  GetStoreData(file:File , model: StoreformData){
    let formParams = new FormData();
    formParams.append('file', file);
    formParams.append('goodId', model.goodId?.toString());
    formParams.append('fromDate',model.fromDate);
    formParams.append('toDate', model.toDate);
    
    return this.http.post<StoreRetData>(this.baseUrl,formParams);
  }

}
