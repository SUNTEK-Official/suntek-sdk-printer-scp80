//
//  ViewController.m
//  iOS_SDKDemo
//
//  Created by yhb on 2021/3/29.
//

#import "ViewController.h"
#import "ScanViewController.h"
#import "CpclViewController.h"
#import "TscViewController.h"
#import "EscViewController.h"

@interface ViewController ()<UITableViewDelegate, UITableViewDataSource>

@property (weak, nonatomic) IBOutlet UITableView *tableview;

@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    if (Manager.bleManager == nil) {
        Manager.bleManager = [BleManager new];
        Manager.bleManager.readTotalCallback = ^(NSData * _Nullable data) {
            
        };
    }
    
    [self setupUI];
    
    [self setupObserver];
    
}

#pragma mark - Observer
- (void)setupObserver {
    WeakSelf(self);
    [[NSNotificationCenter defaultCenter]addObserverForName:NotificationAtDidConnectPeripheral object:nil queue:nil usingBlock:^(NSNotification * _Nonnull note) {
        [weakself.tableview reloadData];
    }];
    [[NSNotificationCenter defaultCenter]addObserverForName:NotificationAtDidDisconnectPeripheral object:nil queue:nil usingBlock:^(NSNotification * _Nonnull note) {
        [weakself.tableview reloadData];
    }];
}


#pragma mark - UI
- (void)setupUI {
    
    {
        self.title = @"Demo";
    }
    
    {
        self.tableview.delegate = self;
        self.tableview.dataSource = self;
        [self.tableview registerClass:[UITableViewCell class] forCellReuseIdentifier:NSStringFromClass([UITableViewCell class])];
    }
    
}

#pragma mark - UITableViewDelegate
- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    
    [tableView deselectRowAtIndexPath:indexPath animated:false];
    
    if (indexPath.section == 0) {
        if (Manager.bleManager.isConnected) {
            UIAlertController *alert = [UIAlertController alertControllerWithTitle:@"Tips" message:@"Disconnect from the device?" preferredStyle:(UIAlertControllerStyleAlert)];
            [alert addAction:[UIAlertAction actionWithTitle:@"YES" style:(UIAlertActionStyleDefault) handler:^(UIAlertAction * _Nonnull action) {
                [Manager.bleManager cancelPeripheralConnection:[Manager.bleManager getConnectedDevice]];
            }]];
            [alert addAction:[UIAlertAction actionWithTitle:@"NO" style:(UIAlertActionStyleCancel) handler:nil]];
            [self presentViewController:alert animated:YES completion:nil];
            
        } else {
            ScanViewController *scanVC = [[UIStoryboard storyboardWithName:@"Main" bundle:nil]instantiateViewControllerWithIdentifier:@"ScanViewController"];
            [self.navigationController pushViewController:scanVC animated:YES];
        }
    } else {
        switch (indexPath.row) {
            case 0:{
                CpclViewController *cpclVC = [[UIStoryboard storyboardWithName:@"Main" bundle:nil]instantiateViewControllerWithIdentifier:@"CpclViewController"];
                [self.navigationController pushViewController:cpclVC animated:YES];
            }
                break;
            case 1:{
                TscViewController *tscVC = [[UIStoryboard storyboardWithName:@"Main" bundle:nil]instantiateViewControllerWithIdentifier:@"TscViewController"];
                [self.navigationController pushViewController:tscVC animated:YES];
            }
                break;
            case 2:{
                EscViewController *escVC = [[UIStoryboard storyboardWithName:@"Main" bundle:nil]instantiateViewControllerWithIdentifier:@"EscViewController"];
                [self.navigationController pushViewController:escVC animated:YES];
            }
                break;
            default:
                break;
        }
    }
}

#pragma mark - UITableViewDataSource
- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    return 2;
}

- (nullable NSString *)tableView:(UITableView *)tableView titleForHeaderInSection:(NSInteger)section {
    if (section == 0) {
        return @"State";
    }
    return @"Command";
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    if (section == 0) {
        return 1;
    }
    return 3;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:NSStringFromClass([UITableViewCell class]) forIndexPath:indexPath];
    if (indexPath.section == 0) {
        cell.textLabel.text = Manager.bleManager.isConnected ? [NSString stringWithFormat:@"Conneted Device:%@",[Manager.bleManager getConnectedDevice].name] : @"Disconnet";
    } else {
        switch (indexPath.row) {
            case 0:{
                cell.textLabel.text = @"CPCL";
            }
                break;
            case 1:{
                cell.textLabel.text = @"TSPL";
            }
                break;
            case 2:{
                cell.textLabel.text = @"ESC/POS";
            }
                break;
            default:
                break;
        }
    }
    return cell;
}

@end
