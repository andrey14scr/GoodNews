import { Injectable } from '@angular/core';
import { NEWS } from 'src/mocks/mock-news';
import { Article } from 'src/models/article';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpRequest } from '@angular/common/http';
import * as queryString from 'query-string';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NewsService {

  constructor(private http: HttpClient) { }

  getNews(): Observable<Article[]>{
    return this.get('Article'+ '?skip=0&take=10&sortBy=0').pipe();
  }

  getById(id: string){
    return this.get('Article/' + id).pipe();
  }

  private url(url: string) {
    return environment.apiUrl + url;
  }

  get(url: string, data: object = {}): Observable<any> {
    if (Object.keys(data).length > 0) {
      url += '?' + queryString.stringify(data);
    }
    return this.http.get(this.url(url));
  }

  post(url: string, data: object, options: object = {}): Observable<any> {
    return this.http.post(this.url(url), data, options);
  }

  patch(url: string, data: object, options: object = {}): Observable<any> {
    return this.http.patch(this.url(url), data, options);
  }

  delete(url: string): Observable<any> {
    return this.http.delete(this.url(url));
  }

  put(url: string, data: object): Observable<any> {
    return this.http.put(this.url(url), data);
  }

  request(method: string, url: string, data: object, options: object): Observable<any> {
    const req = new HttpRequest(method, this.url(url), data, options);
    return this.http.request(req);
  } 
}
