import { Injectable } from '@angular/core';
import { ProductType } from '../models/ProductType.enum';
import { IGasStation } from '../models/IGasStation';
import { Observable, Subscription } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';

@Injectable()
export class DataSetService {
  private gasStationsList: IGasStation[] = [
    {
      id: 1,
      name: "УкрНефть",
      latitude: 48.65026734474433,
      longitude: 22.26973119096572,
      productType: ProductType.Diesel,
      workTimeFrom: 0,
      workTimeTo: 24
    },
    {
      id: 2,
      name: "Okko",
      latitude: 48.62303701196466,
      longitude: 22.308591782925046,
      productType: ProductType.Electro,
      workTimeFrom: 9,
      workTimeTo: 22
    },
    {
      id: 3,
      name: "Azs Market",
      latitude: 48.6262608358449,
      longitude: 22.31121016702216,
      productType: ProductType.Gas,
      workTimeFrom: 0,
      workTimeTo: 24
    },
    {
      id: 4,
      name: "Ultra",
      latitude: 48.62870018851932,
      longitude: 22.335156782925043,
      productType: ProductType.Electro,
      workTimeFrom: 0,
      workTimeTo: 24
    },
    {
      id: 5,
      name: "Wog",
      latitude: 48.61268627235552,
      longitude: 22.27650961248497,
      productType: ProductType.Gas,
      workTimeFrom: 9,
      workTimeTo: 22
    },
    {
      id: 6,
      name: "BVS",
      latitude: 48.6244120085862,
      longitude: 22.25546224429075,
      productType: ProductType.Diesel,
      workTimeFrom: 9,
      workTimeTo: 22
    }
  ]
  private stationsUrl = 'https://localhost:7077/api/Stations/';
  private subscription!: Subscription;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient){

    }

  public getAllStations(): Observable<IGasStation[]> {
    return this.http.get<IGasStation[]>(this.stationsUrl + "all");
  }

  public getStationById(id: number): Observable<IGasStation> {
    return this.http.get<IGasStation>(this.stationsUrl + id.toString());
  }

  public addNewStation(value: IGasStation):Observable<IGasStation>{
    return this.http.post<IGasStation>(this.stationsUrl, JSON.stringify(value), this.httpOptions);
  }

  public updateGasStation(id: number, value: IGasStation):Observable<IGasStation>{
    return this.http.put<IGasStation>(this.stationsUrl + id, JSON.stringify(value), this.httpOptions);
  }

  public updateStationsList(){
    this.subscription = this.getAllStations()
      .subscribe(stations => this.gasStationsList = stations);
  }

  public deleteStation(id: number):Observable<any>{
    return this.http.delete<any>(this.stationsUrl + id);
  }
}
