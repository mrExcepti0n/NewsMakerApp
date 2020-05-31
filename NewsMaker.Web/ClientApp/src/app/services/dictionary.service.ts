import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { DictionaryItem } from "../models/dictionary_item.model";


@Injectable()
export class DictionaryService {
  private url = "/api/Dictionary";

  constructor(private http: HttpClient) {
  }

  getCategoryDictionary(): Observable<DictionaryItem[]> {
    let parameters = new HttpParams().set('refType','1');
    return this.http.get<DictionaryItem[]>(this.url, { params: parameters });
  }
}
