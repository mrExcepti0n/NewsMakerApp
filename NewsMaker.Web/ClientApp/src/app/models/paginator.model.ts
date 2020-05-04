export class Paginator {

  constructor(private _totalElements: number = 0, elementsPerPage: number = 5) {

    this.elementPerPage = elementsPerPage;   
  }

  currentPage: number;
  totalPages: number;

  private _elementPerPage: number;

  get elementPerPage(): number {
    return this._elementPerPage;
  }
   

  set elementPerPage(value: number) {
    this.currentPage = 1;    
    this._elementPerPage = value;
    this.totalPages = 3;

      //Math.ceil(this._totalElements / this._elementPerPage);
  }

  get take(): number {
    return this._elementPerPage;
  }

  get skip(): number {
    return this._elementPerPage * (this.currentPage - 1);
  }
}
