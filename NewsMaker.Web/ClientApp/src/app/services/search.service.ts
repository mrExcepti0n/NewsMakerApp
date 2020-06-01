import { Injectable } from '@angular/core'
import { HttpClient, HttpParams } from '@angular/common/http'
import {SearchResult} from '../models/searchResult'
import { Observable } from 'rxjs';

@Injectable()

export class SearchService {
  private url = "/api/Search";

  constructor(private http: HttpClient) {

  }

  find(pattern: string): Observable<SearchResult> {
    let parameters = new HttpParams().set('pattern', pattern);

    return this.http.get<SearchResult>(this.url, { params: parameters });
  }
}
