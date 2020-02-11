import { Injectable } from '@angular/core'
import { HttpClient, HttpParams } from '@angular/common/http'
import { SearchResult } from '../models/searchResult'
import { Observable } from 'rxjs';
import { NewsDto } from '../models/news.model';

@Injectable()

export class NewsService {
  private url = "/api/News"

  constructor(private http: HttpClient) {

  }

  addNews(news: NewsDto): Observable<number> {
    return this.http.post<number>(this.url, news);
  }


  updateNews(news: NewsDto): Observable<NewsDto> {
    return this.http.put<NewsDto>(this.url, news);
  }


  removeNews(id: number): Observable<any> {
    return this.http.delete(`${this.url}/${id}`);
  }


  getNewsCollection(): Observable<NewsDto[]> {
    var res = this.http.get<NewsDto[]>(this.url);
    return res;
  }

  getNews(id: number): Observable<NewsDto> {
    return this.http.get<NewsDto>(`${this.url}/${id}`);
  }
}
