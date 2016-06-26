# Files contained:
- Source code: 
	C# Win Form Application
- country.csv
	Country data input file when running application
- agent.txt
	Agent name text file, it might be fake data
- proxy.txt
	Proxy text file contains some proxy can be used when create request to server

# Requirement
https://visapoint.eu/form
đây là trang đặt lịch pv visa vào châu Âu
và nếu chọn bất kỳ quốc gia nào
thì luôn có slot
nhưng việt nam thì không
vì việt nam có các tool và hệ thống chạy để đặt visa tự động

Luật như sau: Mỗi tuần sẽ có 20 slot được available ra vào đúng giây thứ 00 của 1 phút bất kỳ trong tuần
và chúng ta cần phải submit dữ liệu lên đúng vào giây 00
or sau giây 00 cỡ 5-10ms
như vậy mới tranh giành slot với các tool khác được
vì hiện tại tool này
đang bị tool khác chiếm mất slot
trước thì có ngon lành
và việc send nhiều request từ 1 đầu IP or proxy sẽ bi block
cụ thể nếu >=2 request/ phút
thì sau 5 phút sẽ bị block
mõi lần block là 3 tiếng

# Current status & Problems
3 class
1 class để decapt cha
1 để encode
1 để tạo thread or request
ý tưởng đơn giản
đầu tiên cần tính độ lệch giữa time client
và server visa kia
nhưng server visa kia
chỉ trả về đến mức giây
không phải milisecond
sau đó cần tính toán time để send request và lấy results về
về căn ước time cần submit data form lên
chỉ cần slot available là coi như thành công
vấn đề không phức tạp nhưng chẳng hề đơn giản
trước đây chạy ngon lành
giờ bị các tool khác chiếm mất