import { StoreData } from "./store-data";

export class StoreRetData {
    constructor(
        public transactionNum:number,
        public amountTotal:number,
        public amountRemain:number,

        public goodData:StoreData[],
        
    ){}
}
