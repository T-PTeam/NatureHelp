import { Injectable } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { IDeficiency } from '@/modules/soil-deficiency/models/ISoilDeficiency';

@Injectable()
export class DataSetService {
  private soilsUrl = 'https://localhost:7077/api/Soils/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient){

    }

  public getAllSoilDeficiencies(): Observable<IDeficiency[]> {
    return this.http.get<IDeficiency[]>(this.soilsUrl + "all");
  }

  public getSoilDeficiencyById(id: string): Observable<IDeficiency> {
    return this.http.get<IDeficiency>(this.soilsUrl + id.toString());
  }

  public addNewSoilDeficiency(value: IDeficiency):Observable<IDeficiency>{
    return this.http.post<IDeficiency>(this.soilsUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateSoilDeficiency(id: string, value: IDeficiency):Observable<IDeficiency>{
    return this.http.put<IDeficiency>(this.soilsUrl + id, JSON.stringify(value), this.httpOptions);
  }

  public deleteSoilDeficiency(id: string):Observable<any>{
    return this.http.delete<any>(this.soilsUrl + id);
  }
}
