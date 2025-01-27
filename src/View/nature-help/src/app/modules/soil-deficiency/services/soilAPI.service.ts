import { Injectable } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { ISoilDeficiency } from '@/modules/soil-deficiency/models/ISoilDeficiency';

@Injectable()
export class SoilAPIService {
  private soilsUrl = 'https://localhost:7077/soil';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient){

    }

  public getAllSoilDeficiencies(): Observable<ISoilDeficiency[]> {
    return this.http.get<ISoilDeficiency[]>(this.soilsUrl);
  }

  public getSoilDeficiencyById(id: string): Observable<ISoilDeficiency> {
    return this.http.get<ISoilDeficiency>(this.soilsUrl + id.toString());
  }

  public addNewSoilDeficiency(value: ISoilDeficiency):Observable<ISoilDeficiency>{
    return this.http.post<ISoilDeficiency>(this.soilsUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateSoilDeficiency(id: string, value: ISoilDeficiency):Observable<ISoilDeficiency>{
    return this.http.put<ISoilDeficiency>(this.soilsUrl + id, JSON.stringify(value), this.httpOptions);
  }

  public deleteSoilDeficiency(id: string):Observable<any>{
    return this.http.delete<any>(this.soilsUrl + id);
  }
}
