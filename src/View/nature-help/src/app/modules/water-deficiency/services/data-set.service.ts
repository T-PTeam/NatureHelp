import { Injectable } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { IDeficiency } from '@/modules/water-deficiency/models/IWaterDeficiency';

@Injectable()
export class DataSetService {
  private watersUrl = 'https://localhost:7077/api/Waters/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient){

    }

  public getAllWaterDeficiencies(): Observable<IDeficiency[]> {
    return this.http.get<IDeficiency[]>(this.watersUrl + "all");
  }

  public getWaterDeficiencyById(id: string): Observable<IDeficiency> {
    return this.http.get<IDeficiency>(this.watersUrl + id.toString());
  }

  public addNewWaterDeficiency(value: IDeficiency):Observable<IDeficiency>{
    return this.http.post<IDeficiency>(this.watersUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateWaterDeficiency(id: string, value: IDeficiency):Observable<IDeficiency>{
    return this.http.put<IDeficiency>(this.watersUrl + id, JSON.stringify(value), this.httpOptions);
  }

  public deleteWaterDeficiency(id: string):Observable<any>{
    return this.http.delete<any>(this.watersUrl + id);
  }
}
