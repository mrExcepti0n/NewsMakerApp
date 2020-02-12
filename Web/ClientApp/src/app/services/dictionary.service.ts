import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { KeyValuePair } from "../models/keyValuePair.model";


@Injectable()
export class DictionaryService {
  private url = "/api/Dictionary"

  constructor(private http: HttpClient) {
  }

  getCategoryDictionary(): Observable<KeyValuePair[]> {
    let parameters = new HttpParams().set('refType','1');
    return this.http.get<KeyValuePair[]>(this.url, { params: parameters });
  }
}
