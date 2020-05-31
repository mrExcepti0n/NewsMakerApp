import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs';
import { NewsDto } from '../models/news.model';

@Injectable()

export class NewsService {
  private baseUrl = "/api/News";

  constructor(private http: HttpClient) {

  }

  addNews(news: NewsDto): Observable<number> {
    return this.http.post<number>(this.baseUrl, news);
  }


  updateNews(news: NewsDto): Observable<NewsDto> {
    return this.http.put<NewsDto>(this.baseUrl, news);
  }


  removeNews(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }


  getNewsCollection(categoryId: number = null, skip: number = null, take: number = null): Observable<NewsDto[]> {

    let params: string[] = [];
    if (categoryId !== null) {
      params.push(`categoryId=${categoryId}`);
    }
    if (skip !== null) {
      params.push(`skip=${skip}`);
    }
    if (take !== null) {
      params.push(`take=${take}`);
    }

    let url = this.baseUrl;

    if (params.length > 0) {
      url += '?' + params.join('&');
    }
    return this.http.get<NewsDto[]>(url);
  }

  getNewsCount(categoryId: number = null): Observable<number> {
    let url = this.baseUrl + '/Count';

    if (categoryId !== null) {
      url += `?categoryId=${categoryId}`;
    }

    return this.http.get<number>(url);
  }

  getNews(id: number): Observable<NewsDto> {
    return this.http.get<NewsDto>(`${this.baseUrl}/${id}`);
  }
}
