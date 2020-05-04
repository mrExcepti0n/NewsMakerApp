import { Component } from '@angular/core';
import { SearchService } from '../services/search.service'
import { SearchResult, SearchGroupResult } from '../models/searchResult';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  providers: [SearchService]
})
export class HomeComponent {

  public searchResult: SearchResult = new SearchResult([]);

  constructor(private service: SearchService) {   
  }


  search(pattern: string) {
    this.service.find(pattern).subscribe(res => { this.searchResult = res; });
  }
}
