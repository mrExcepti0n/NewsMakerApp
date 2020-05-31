import { Subject, Observable } from "rxjs";
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { PostComment } from "../comments/models/post-comment";
import { filter } from 'rxjs/operators';

export class SignalrService {
  private _hubConnection: HubConnection;
  private signalrHubUrl: string = 'http://localhost:5000';


  private msgSignalrSource = new Subject<PostComment>();
  msgReceived$ = this.msgSignalrSource.asObservable();

  constructor() {
    this.init();
  }

  private init() {
      this.register();
      this.stablishConnection();
      this.registerHandlers();
  }

  private register() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(this.signalrHubUrl + '/hub/notificationhub')
      .configureLogging(LogLevel.Trace)
      .withAutomaticReconnect()
      .build();
  }

  private stablishConnection() {
    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started');
      })
      .catch((res) => {
        console.log('Error while establishing connection ' + res);
      });
  }

  private registerHandlers() {
    this._hubConnection.on('PostComment', (msg) => {
      this.msgSignalrSource.next(msg);
    });
  }

  ///todo message sended before connection started
  public joinGroup(postId: number): Observable<PostComment> {
    console.log('joinGroup');
    this._hubConnection.send('JoinGroup', postId);
    return this.msgReceived$.pipe(filter(pc => pc.postId == postId));
  }

  public leaveGroup(postId: number) {
    console.log('leaveGroup');
    this._hubConnection.send('LeaveGroup', postId);
  }
}
