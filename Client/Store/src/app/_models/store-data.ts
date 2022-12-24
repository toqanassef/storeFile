export class StoreData {
    constructor(
        public goodId:number,
        public transactionId:number,
        public transactionDate:Date,
        public amount:number,
        public direction:string,
        public comment?:string,
    ){}
}