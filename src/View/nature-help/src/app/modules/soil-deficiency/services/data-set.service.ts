import { Injectable } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { ISDeficiency } from '@/modules/soil-deficiency/models/ISoilDeficiency';

@Injectable()
export class DataSetService {
  private soilsUrl = 'https://localhost:7077/api/Soils/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient){

    }

  public getAllSoilDeficiencies(): Observable<ISDeficiency[]> {
    return this.http.get<ISDeficiency[]>(this.soilsUrl + "all");
  }

  public getSoilDeficiencyById(id: string): Observable<ISDeficiency> {
    return this.http.get<ISDeficiency>(this.soilsUrl + id.toString());
  }

  public addNewSoilDeficiency(value: ISDeficiency):Observable<ISDeficiency>{
    return this.http.post<ISDeficiency>(this.soilsUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateSoilDeficiency(id: string, value: ISDeficiency):Observable<ISDeficiency>{
    return this.http.put<ISDeficiency>(this.soilsUrl + id, JSON.stringify(value), this.httpOptions);
  }

  public deleteSoilDeficiency(id: string):Observable<any>{
    return this.http.delete<any>(this.soilsUrl + id);
  }
}
