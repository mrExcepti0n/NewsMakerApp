import { Subject } from "rxjs";
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { PostComment } from "../comments/models/post-comment";


export class SignalrService {
  private _hubConnection: HubConnection;
  private SignalrHubUrl: string = '';


  private msgSignalrSource = new Subject<PostComment>();
  msgReceived$ = this.msgSignalrSource.asObservable();


  private init() {
      this.register();
      this.stablishConnection();
      this.registerHandlers();
  }

  private register() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(this.SignalrHubUrl + '/hub/notificationhub')
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();
  }

  private stablishConnection() {
    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started')
      })
      .catch(() => {
        console.log('Error while establishing connection')
      });
  }

  private registerHandlers() {
    this._hubConnection.on('UpdatedOrderState', (msg) => {
      this.msgSignalrSource.next(msg);
    });
  }
}
