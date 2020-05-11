import { Subject } from "rxjs";

export class PagingService {

  constructor() {
    this._totalItemsCount = 0;
    this._currentPage = 1;
    this._elementsPerPage = 5;
  }
  private _totalItemsCount: number;

  public get totalItemsCount() {
    return this._totalItemsCount;
  }

  public set totalItemsCount(value: number) {
    this._totalItemsCount = value;
    this.totalItemsChangedSource.next(value);
  }


  private _currentPage: number;

  public get currentPage() {
    return this._currentPage;
  }

  public set currentPage(value: number) {
    this._currentPage = value;
    this.currentPageChangedSource.next(value);
  }


  private _elementsPerPage: number;
  public get elementsPerPage() {
    return this._elementsPerPage;
  }

  public set elementsPerPage(value: number) {
    this._elementsPerPage = value;
    this._currentPage = 1;
    this.elementsPerPageChangedSource.next(value);
  }

  public get take() {
    return this.elementsPerPage;
  }

  public get skip() {
    return this.elementsPerPage * (this.currentPage - 1);
  }

  private totalItemsChangedSource = new Subject<number>();
  public totalItemsChanged$ = this.totalItemsChangedSource.asObservable();

  private currentPageChangedSource = new Subject<number>();
  public currentPageChanged$ = this.currentPageChangedSource.asObservable();

  private elementsPerPageChangedSource = new Subject<number>();
  public elementsPerPageChanged$ = this.elementsPerPageChangedSource.asObservable();
}
