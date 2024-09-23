import { TestBed} from '@angular/core/testing';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppComponent],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have the 'HackerNewsApp.Web' title`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('HackerNewsApp.Web');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Hacker News Newest Stories');
  });

  describe('should update page number when button onClick is triggered', () => {
    it('button clicked', () => {
      const fixture = TestBed.createComponent(AppComponent);
      fixture.componentInstance.onPageChange(2);
      expect(fixture.componentInstance.pageNumber).toBe(2);
    });
  });
  
  describe('should update the search term when search term input is triggered', () => {
    it('search input triggered', () => {
      const fixture = TestBed.createComponent(AppComponent);
      const mockEvent: Event = <Event><any>{
        target: {
            value: "abc"      
        }
      };
      fixture.componentInstance.onSearch(mockEvent);
      expect(fixture.componentInstance.searchTerm).toBe("abc");
    });
  });
});

