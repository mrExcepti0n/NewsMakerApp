import { Component } from "@angular/core";
import { PagingService } from "../services/paging.service";

@Component({
  selector: 'app-paging',
  templateUrl: 'paging.component.html',
  styleUrls: ['paging.component.css']
})
export class PagingComponent {


  constructor(private pagingService: PagingService) {
    this.totalItems = 0;
    this.currentPage = 1;
    this.elementsPerPage = 5;
  }

  ngOnInit(): void {
    this.totalItems = this.pagingService.totalItemsCount;
    this.pagingService.totalItemsChanged$
      .subscribe(res => this.totalItemsChanged(res));

    this.elementsPerPage = this.pagingService.elementsPerPage;
    this.pagingService.elementsPerPageChanged$
      .subscribe(res => {
        this.elementsPerPage = res;
        this.currentPage = this.pagingService.currentPage;
        this.createPages();
      });

    this.currentPage = this.pagingService.currentPage;
    this.pagingService.currentPageChanged$
      .subscribe(res => this.currentPage = res);

    this.createPages();
  }

  totalItems: number;
  currentPage: number;
  elementsPerPage: number;

  get totalPages() {
    return Math.ceil(this.totalItems / this.elementsPerPage);
  }

  totalItemsChanged(value: number) {
    this.totalItems = value;
    this.createPages();
  }


  pages: number[] = [];



  setPage(currentPage: number) {
    this.pagingService.currentPage = currentPage;
  }

  changePageSize(size: number) {
    this.pagingService.elementsPerPage = size;
    this.elementsPerPage = size;
    this.createPages();
  }


  createPages() {
    if (this.totalPages !== this.pages.length) {
      this.pages = [];
      for (let i = 1; i <= this.totalPages; i++) {
        this.pages.push(i);
      }
    }
  }
}
