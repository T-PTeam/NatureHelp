import { Injectable } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { IWaterDeficiency } from '@/modules/water-deficiency/models/IWaterDeficiency';

@Injectable()
export class WaterAPIService {
  private watersUrl = 'https://localhost:7077/water/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient){

    }

  public getAllWaterDeficiencies(): Observable<IWaterDeficiency[]> {
    return this.http.get<IWaterDeficiency[]>(this.watersUrl);
  }

  public getWaterDeficiencyById(id: string): Observable<IWaterDeficiency> {
    return this.http.get<IWaterDeficiency>(this.watersUrl + id.toString());
  }

  public addNewWaterDeficiency(value: IWaterDeficiency):Observable<IWaterDeficiency>{
    return this.http.post<IWaterDeficiency>(this.watersUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateWaterDeficiency(id: string, value: IWaterDeficiency):Observable<IWaterDeficiency>{
    return this.http.put<IWaterDeficiency>(this.watersUrl + id, JSON.stringify(value), this.httpOptions);
  }

  public deleteWaterDeficiency(id: string):Observable<any>{
    return this.http.delete<any>(this.watersUrl + id);
  }
}
