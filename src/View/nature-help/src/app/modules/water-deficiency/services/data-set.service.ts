import { Injectable } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { IWDeficiency } from '@/modules/water-deficiency/models/IWaterDeficiency';

@Injectable()
export class DataSetService {
  private watersUrl = 'https://localhost:7077/api/Waters/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient){

    }

  public getAllWaterDeficiencies(): Observable<IWDeficiency[]> {
    return this.http.get<IWDeficiency[]>(this.watersUrl + "all");
  }

  public getWaterDeficiencyById(id: string): Observable<IWDeficiency> {
    return this.http.get<IWDeficiency>(this.watersUrl + id.toString());
  }

  public addNewWaterDeficiency(value: IWDeficiency):Observable<IWDeficiency>{
    return this.http.post<IWDeficiency>(this.watersUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateWaterDeficiency(id: string, value: IWDeficiency):Observable<IWDeficiency>{
    return this.http.put<IWDeficiency>(this.watersUrl + id, JSON.stringify(value), this.httpOptions);
  }

  public deleteWaterDeficiency(id: string):Observable<any>{
    return this.http.delete<any>(this.watersUrl + id);
  }
}
